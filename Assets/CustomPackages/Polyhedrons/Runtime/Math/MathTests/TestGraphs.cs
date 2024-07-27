using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEditor.VersionControl;
//using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.TestTools;

public class TestGraphs
{
    // A Test behaves as an ordinary method
    [Test]
    public void TestDFS()
    {
        SimpleGraph graph = new SimpleGraph();
        // Edges are arranged so that the position of the nodes in a DFS will be the same as their indices
        var root = graph.CreateGraph(6, new List<(int, int)>{
            (0,1),(0,2),(1,2),(2,3), (0,4),(0,5),(4,5)
        });

        List<(int, int, int, int, EdgeTravelType)> callbackInfo = new List<(int, int, int, int, EdgeTravelType)>{
            // in 0
            (0, 0, 1, 1, EdgeTravelType.FIRST_DISCOVER),
            // in 1
            (1, 1, 0, 0, EdgeTravelType.TO_PARENT),
            (1, 1, 2, 2, EdgeTravelType.FIRST_DISCOVER),
            // in 2
            (2, 0, 0, 0, EdgeTravelType.ALREADY_DISCOVERED),
            (2, 0, 1, 1, EdgeTravelType.TO_PARENT),
            (2, 0, 3, 3, EdgeTravelType.FIRST_DISCOVER),
            // in 3
            (3, 3, 2, 2, EdgeTravelType.TO_PARENT),
            // back to 2
            (2, 0, 3, 3, EdgeTravelType.AFTER_DISCOVER),
            // back to 1
            (1, 1, 2, 0, EdgeTravelType.AFTER_DISCOVER),
            // back to 0
            (0, 0, 1, 0, EdgeTravelType.AFTER_DISCOVER),
            (0, 0, 2, 0, EdgeTravelType.ALREADY_DISCOVERED),
            (0, 0, 4, 4, EdgeTravelType.FIRST_DISCOVER),

            // in 4
            (4, 4, 0, 0, EdgeTravelType.TO_PARENT),
            (4, 4, 5, 5, EdgeTravelType.FIRST_DISCOVER),
            // in 5
            (5, 0, 0, 0, EdgeTravelType.ALREADY_DISCOVERED),
            (5, 0, 4, 4, EdgeTravelType.TO_PARENT),
            // back to 4
            (4, 4, 5, 0, EdgeTravelType.AFTER_DISCOVER),
            // back to 0
            (0, 0, 4, 0, EdgeTravelType.AFTER_DISCOVER),
            (0, 0, 5, 5, EdgeTravelType.ALREADY_DISCOVERED),
        };

        var callbackEnumerator = callbackInfo.GetEnumerator();

        void PrintEdgeTravel(
            SimpleGraphNode fromNode, int fromPosition, int fromSmallestVisited,
            SimpleGraphNode toNode, int toPosition, int toSmallestVisited,
            SimpleGraphEdge edge, EdgeTravelType travelType)
        {
            Assert.AreEqual(fromNode.index, fromPosition);
            Assert.AreEqual(toNode.index, toPosition);
            //Debug.Log($"Edge {fromNode}->{toNode}, smallest {fromSmallestVisited}->{toSmallestVisited},  {travelType})");
            //Debug.Log($"({fromPosition}, {fromSmallestVisited}, {toPosition}, {toSmallestVisited}, EdgeTravelType.{travelType}),");
            Assert.IsTrue(callbackEnumerator.MoveNext(), "Too many callbacks");
            Assert.AreEqual(callbackEnumerator.Current, (fromPosition, fromSmallestVisited, toPosition, toSmallestVisited, travelType));
            
        }

        var graphAlgo = new GraphAlgo<SimpleGraphNode, SimpleGraphEdge>(graph);
        graphAlgo.DFS(root, PrintEdgeTravel);
    }

    [Test]
    public void TestGetDirectedEdges()
    {
        SimpleGraph graph = new SimpleGraph();
        // Edges are arranged so that the position of the nodes in a DFS will be the same as their indices
        var root = graph.CreateGraph(6, new List<(int, int)>{
            (0,1),(0,2),(1,2),(2,3), (0,4),(0,5),(4,5)
        });

        HashSet<(int, int, bool, bool)> edgeInfo = new HashSet<(int, int, bool, bool)>{
            (2, 3, true,  true),
            (1, 2, true,  false),
            (0, 1, true,  false),
            (0, 2, false, false),
            (4, 5, true,  false),
            (0, 4, true,  false),
            (0, 5, false, false),
        };

        var graphAlgo = new GraphAlgo<SimpleGraphNode, SimpleGraphEdge>(graph);
        var directedEdges = graphAlgo.GetDirectedEdgesDFS(root);
        HashSet<(int, int, bool, bool)> resultSet = new HashSet<(int, int, bool, bool)>(
            from edge in directedEdges select (edge.parent.index, edge.child.index, edge.treeEdge, edge.seperatorEdge)
        );
        Assert.IsTrue(edgeInfo.SetEquals(resultSet));
        /*foreach (var edge in directedEdges) {
            Debug.Log($"({edge.parent.index}, {edge.child.index}, {edge.treeEdge}, {edge.seperatorEdge}),");
        }*/
    }

    /*
    [Test]
    public void TestCycleStronglyConnectedComponents()
    {
       // single connected component: cycle
       SimpleGraph simpleGraph = new SimpleGraph();
       var root = simpleGraph.CreateGraph(5, new List<(int, int)>{
           (0,1),(1,2),(2,3),(3,0)
       });

       Dictionary<SimpleGraphNode, int> compIndex = simpleGraph.GetStronglyConnectedComponents(root);

       int index = compIndex[root];
       foreach (KeyValuePair<SimpleGraphNode, int> key_value in compIndex)
       {
           Debug.Log(key_value);
           Assert.AreEqual(index, key_value.Value);
       }

    }*/

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    /*[UnityTest]
    public IEnumerator TestGraphsWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }*/
}
