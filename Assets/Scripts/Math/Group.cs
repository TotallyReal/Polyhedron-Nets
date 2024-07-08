using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using UnityEngine;

abstract public class Group<Element> : IEnumerable<Element>
{

    abstract public Element Identity();

    abstract public bool Contains(Element elem);

    abstract public bool AreEqual(Element elem1, Element elem2);

    abstract public Element Multiply(Element elem1, Element elem2);

    abstract public Element Inverse(Element elem);

    abstract public IEnumerable<Element> CosetRepresentatives(Group<Element> subgroup);

    abstract public IEnumerator<Element> GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
}


/// <summary>
/// Using a finite multiplication table to implement the group structure.
/// </summary>
/// <typeparam name="Element"></typeparam>
abstract public class FiniteGroup<Element> : Group<Element>
{
    private readonly HashSet<Element> groupElements; // TODO: check what is the hash of a general "ELEMENT".
    private readonly Dictionary<(Element, Element), Element> multiplication;
    private readonly Dictionary<Element, Element> inverseOf;
    private readonly Element identity;

    public const int MAX_ITERATIONS = 10;

    public FiniteGroup(){
        identity = CreateIdentity();

        groupElements = new HashSet<Element>{ identity };

        multiplication = new Dictionary<(Element, Element), Element> { 
            { (identity, identity), identity }
        };

        inverseOf = new Dictionary<Element, Element> { { identity, identity } };
    }

    #region ------------------------------ identity ------------------------------

    abstract protected Element CreateIdentity();

    public override Element Identity()
    {
        return identity;
    }

    #endregion

    //TODO - consider updating the whole group (elements, inverse and multiplication) whenever we add a new element to it.
    #region ------------------------------ inclusion ------------------------------

    public override bool Contains(Element elem)
    {
        return groupElements.Contains(elem);
    }

    /// <summary>
    /// Return the cuurent number of elements added to the group (and not necessarily the full group size).
    /// </summary>
    /// <returns></returns>
    public int CurrentNumberOfElement()
    {
        return groupElements.Count;
    }

    /// <summary>
    /// Try to find if the group already contains an "equivalent" element. If so returns true and the equivalent element
    /// in the out parameter. Otherwise returns false.
    /// </summary>
    /// <param name="elem"></param>
    /// <returns></returns>
    public abstract bool TryGet(Element elem, out Element elemInGroup);

    /// <summary>
    /// Tries to add the given element to this group, and returns it. Might return a different (but equivalent) element
    /// than the one in the paramter.
    /// </summary>
    /// <param name="elem">The element to add to the group</param>
    /// <returns>The element added to the group</returns>
    public Element AddElement(Element elem)
    {        
        if (TryGet(elem, out Element element))
            return element;

        groupElements.Add(elem);
        return elem;
    }


    /// <summary>
    /// Returns the current number of elements in the group (might not be the full group size).
    /// </summary>
    /// <returns></returns>
    public int Size()
    {
        return groupElements.Count;
    }

    #endregion

    #region ------------------------------ enumeration ------------------------------
    public override IEnumerator<Element> GetEnumerator()
    {
        return groupElements.GetEnumerator();
    }

    #endregion

    #region ------------------------------ multiplication ------------------------------

    public override Element Multiply(Element elem1, Element elem2)
    {
        if (!Contains(elem1) || !Contains(elem2))
        {
            Debug.LogError("Trying to multiply elements which do not belong to this group");
            throw new System.Exception("Trying to multiply elements which do not belong to this group");
            // return null;  // TODO: check if there is a reason I put here return null instead of exception
        }
        if (multiplication.TryGetValue((elem1, elem2), out Element result))
        {
            return result;
        }

        result = AddElement(SimpleMultiply(elem1, elem2));
        multiplication.Add((elem1, elem2), result);
        return result;
    }

    /// <summary>
    /// Just multiply the two elements, without checking if they are in the group or adding their result to it.
    /// </summary>
    /// <param name="elem1"></param>
    /// <param name="elem2"></param>
    /// <returns></returns>
    abstract protected Element SimpleMultiply(Element elem1, Element elem2);

    public override Element Inverse(Element elem)
    {
        if (!Contains(elem))
        {
            Debug.LogError($"The group does not contain the element {elem}");
            throw new System.Exception($"The group does not contain the element {elem}");
            // return null;  // TODO: check if there is a reason I put here return null instead of exception
        }

        if (inverseOf.TryGetValue(elem, out Element elemInverse))
            return elemInverse;

        // Could not find inverse. Under the assumption that this is a (small) finite group, 
        // simply take all the powers elem^d until we find one with available inverse. Since the group
        // is assumed to be finite, eventually we should get the identity which is its own inverse.
        // Once we know the inverse g for elem^d, we can find the inverse for elem^{d-i} which is g*elem^i.
        // Compute these and add them to the inverse dictionary.

        List<Element> elementPowers = new List<Element>();
        Element lastElement = elem;
        int lastElementOrder;
        Element lastElementInverse = identity;
        for (lastElementOrder=1; lastElementOrder < MAX_ITERATIONS; lastElementOrder++)
        {
            if (inverseOf.TryGetValue(lastElement, out lastElementInverse))
                break;
            elementPowers.Add(lastElement);

            lastElement = Multiply(lastElement, elem);
        }

        if (lastElementOrder == MAX_ITERATIONS)
        {
            Debug.LogError($"The element {elem} order is too large");
            throw new System.Exception($"The element {elem} order is too large");
            // return null;  // TODO: check if there is a reason I put here return null instead of exception
        }

        elementPowers.Reverse();

        // At this point, elementsPowers = {elem^{d-1}, elem^{d-2}, ..., elem^1}
        // and (elem^d)^{-1} = lastElementInverse

        foreach (var lastElementPower in elementPowers)
        {
            lastElementInverse = Multiply(lastElementInverse, elem);
            inverseOf.Add(lastElementPower, lastElementInverse);
        }

        return lastElementInverse;
    }

    #endregion

    #region ------------------------------ subgroups ------------------------------

    /// <summary>
    /// Compute the subgroup generated by the given elements, under the assumption that it is (small...) finite.
    /// </summary>
    /// <param name="generators"></param>
    /// <returns></returns>
    public Group<Element> Subgroup(params Element[] generators)
    {
        HashSet<Element> subgroup = new HashSet<Element> { identity };
        List<Element> sphere = new List<Element> { identity };

        List<Element> nextSphere = new List<Element>();
        for (int i = 0; i < MAX_ITERATIONS; i++)
        {
            foreach (var sphereElement in sphere)
            {
                foreach (var generator in generators)
                {
                    Element product = Multiply(generator, sphereElement);
                    if (!subgroup.Contains(product))
                    {
                        subgroup.Add(product);
                        nextSphere.Add(product);
                    }
                }
            }

            if (nextSphere.Count == 0)
                break; // no new element
            sphere = nextSphere;
            nextSphere = new List<Element>();
        }

        // TODO: how do I make sure that the subgroup of this finite group is also finite?
        return new Subgroup<Element>(this, subgroup.Contains);
    }

    /// <summary>
    /// Find a list of left coset representatives {g_1,...,g_k} for the given subgroup,
    /// namely g_1*subgroup, ..., g_k*subgroup are disjoint and their union is the whole group.
    /// 
    /// Assumes that this group already contains all of its elements!
    /// 
    /// </summary>
    /// <param name="subgroup"></param>
    /// <returns></returns>
    public override IEnumerable<Element> CosetRepresentatives(Group<Element> subgroup)
    {
        HashSet<Element> representatives = new HashSet<Element>(); // TODO consider adding a comparator based on the group
        foreach (Element element in this)
        {
            if (!TryFindCosetRepresentative(element, representatives, subgroup, out Element g))
                representatives.Add(element);
        }
        return representatives;
    }

    /// <summary>
    /// Looks for a represntative g from the given list such that element*subgroup == g*subgroup.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="cosetsRep"></param>
    /// <param name="subgroup"></param>
    /// <returns></returns>
    public bool TryFindCosetRepresentative(
        Element element, IEnumerable<Element> cosetsRep, Group<Element> subgroup, out Element g)
    {
        //if (element == null)
        //    return element;
        g = Identity();
        Element inverseElement = Inverse(element);
        foreach (var representative in cosetsRep)
        {
            if (subgroup.Contains(Multiply(inverseElement, representative)))
            {
                g = representative;
                return true;
            }
        }
        return false;
    }

    #endregion
}

public class Subgroup<Element> : Group<Element>
{

    public delegate bool ContainsElement(Element elem);

    public static ContainsElement AlsoCheckFullGroup(Group<Element> fullGroup, ContainsElement containsElement)
    {
        return (elem => fullGroup.Contains(elem) && containsElement(elem));
    }

    private readonly Group<Element> containingGroup;
    private readonly ContainsElement filter;

    public Subgroup(Group<Element> containingGroup, ContainsElement filter)
    {
        this.containingGroup = containingGroup;
        this.filter = filter;
    }

    public override bool Contains(Element elem)
    {
        return filter(elem); // TODO - should we check if it is in the containing group, or assume that it is part of the filter?
    }

    public override bool AreEqual(Element elem1, Element elem2)
    {
        return this.containingGroup.AreEqual(elem1, elem2);
    }

    public override IEnumerable<Element> CosetRepresentatives(Group<Element> subgroup)
    {
        foreach (var representative in containingGroup.CosetRepresentatives(subgroup))
        {
            if (Contains(representative))
                yield return representative;
        }
    }

    public override Element Identity()
    {
        return containingGroup.Identity();
    }

    public override Element Inverse(Element elem)
    {
        return containingGroup.Inverse(elem);
    }

    public override Element Multiply(Element elem1, Element elem2)
    {
        return containingGroup.Multiply(elem1, elem2);
    }

    public override IEnumerator<Element> GetEnumerator()
    {
        foreach (var element in containingGroup)
        {
            if (filter(element))
                yield return element;
        }
    }
    /*private FiniteGroup<Element> containingGroup;

public FiniteSubgroup(FiniteGroup<Element> containingGroup)
{
this.containingGroup = containingGroup;
}

public override Element TryGet(Element elem)
{
Element element = containingGroup.TryGet(elem);
if (Contains(element))
  return element;
return null;
}

protected override Element CreateIdentity()
{
return containingGroup.Identity();
}

protected override Element SimpleMultiply(Element elem1, Element elem2)
{
return containingGroup.Multiply(elem1, elem2); // TODO - is this how I want to do it?
}*/
}
