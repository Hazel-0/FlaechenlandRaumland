using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Management;

public class Guardian : MonoBehaviour {
    public GameObject world;
    public GameObject roomCenter;
    public GameObject roomTop;


    private bool initialized = false;
    private bool boundaryWarning = false;

    void Start() {
        // get the loader to access guardian stuff
        var loader = XRGeneralSettings.Instance?.Manager?.activeLoader;
        if (loader == null) {
            Debug.LogWarning("Could not get active Loader.");
            return;
        }
        // get the guardian stuff
        var inputSubsystem = loader.GetLoadedSubsystem<XRInputSubsystem>();
        // subscribe to guardian changes
        inputSubsystem.boundaryChanged += InputSubsystem_boundaryChanged;
        
    }

    private void InputSubsystem_boundaryChanged(XRInputSubsystem inputSubsystem) {
        // store the guardian bounding box in a list of vec3
        List<Vector3> boundaryPoints = new List<Vector3>();
        if (inputSubsystem.TryGetBoundaryPoints(boundaryPoints)) {
            initialized = true;
            // translate and rotate the world
            relocatePlayer(boundaryPoints);
        } else {
            if (!boundaryWarning) {
                Debug.LogWarning($"Could not get Boundary Points for Loader");
                boundaryWarning = true;
            }
        }
    }

    void Update() {
        if (!initialized) {
            // same stuff as in start but needed to react to manual relocation (via oculus button)
            var loader = XRGeneralSettings.Instance?.Manager?.activeLoader;
            if (loader != null) {
                var inputSubsystem = loader.GetLoadedSubsystem<XRInputSubsystem>();
                InputSubsystem_boundaryChanged(inputSubsystem);
            }
        }
    }

    private void relocatePlayer(List<Vector3> boundaryPoints) {
        List<Edge> edges = new List<Edge>();
        Vector3 center = new Vector3(0, 0, 0);

        // sum the box vectors and get the center of the box
        for (int i = 0; i < boundaryPoints.ToArray().Length; i++) {
            center += boundaryPoints[i];
            if (i == boundaryPoints.ToArray().Length - 1) {
                edges.Add(new Edge(boundaryPoints[i], boundaryPoints[0]));
            } else {
                edges.Add(new Edge(boundaryPoints[i], boundaryPoints[i + 1]));
            }
        }
        center /= 4.0f;

        // which edge is shorter (0 or 1)?
        int edgeIndex = 1;
        if (edges[0].length < edges[1].length) {
            edgeIndex = 0;
        }
        Edge vRoom = new Edge(center, edges[edgeIndex].location + (edges[edgeIndex].direction / 2.0f));
        Edge room = new Edge(roomCenter.transform.position, roomTop.transform.position);

        // get translation between world_center and guardian_center and relocate world
        Vector3 translation = vRoom.location - room.location;
        world.transform.Translate(translation);
        // get the angle-difference between world_center and guardian_center and rotate world
        float angle = Vector3.SignedAngle(vRoom.direction, room.direction, Vector3.up);
        world.transform.RotateAround(vRoom.location, Vector3.up, -angle);
    }

    class Edge {
        public Vector3 direction;
        public Vector3 location;
        public float length;

        public Edge(Vector3 start, Vector3 end) {
            direction = end - start;
            length = Vector3.Magnitude(direction);
            location = start;
        }
    }

}
