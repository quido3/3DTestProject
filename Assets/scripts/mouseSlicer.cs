using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Cutted parts can be found again by comparing the current vertices to the cut trail points. Both current area and the deleted area can be combined by replacing the points in trail with the rest from the deleted piece

public class mouseSlicer : MonoBehaviour
{
    public GameObject cutArea;
    public SliceTrailer trailer;
    public LineRenderer liner;
    public GameObject meshParent;
    public GameObject meshPref;
    public MeshHandlerScript meshHandler;

    private Vector3 firstOUT;
    private Vector3 firstIN;
    private Vector3 secondIN;
    private Vector3 secondOUT;

    private Vector3 impactPointIN = Vector3.zero, impactPointOUT = Vector3.zero;

    private int inIndex = 0, outIndex = 0;

    private Vector3 d1 = Vector3.zero, d2 = Vector3.zero, d3 = Vector3.zero;

    private Vector3 tmpV = Vector3.zero;

    List<Vector3> centerPoints = new List<Vector3>();

    private bool drawDebug = true;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (centerPoints.Count == 0)
        {
            centerPoints.Add(cutArea.GetComponent<MeshFilter>().sharedMesh.vertices[0]);
        }
        handleMouse();
        drawLine();

        Debug.DrawLine(Vector3.zero, firstOUT, Color.red);
        Debug.DrawLine(Vector3.zero, firstIN, Color.green);
    }

    private void drawLine()
    {
        List<Vector3> trail = trailer.getList();

        if (trail != null)
        {
            liner.SetVertexCount(trail.Count);
            for (int i = 0; i < trail.Count; i++)
            {
                Vector3 v = trail[i];
                v.z = -1;
                liner.SetPosition(i, v);
            }
        }
    }

    private void handleMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            trailer.startTrail(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            trailer.Add(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            trailer.endTrail(Input.mousePosition);
            List<GameObject> hittedmeshes = getHittedMeshes(trailer.getList());
            if (hittedmeshes != null && hittedmeshes.Count > 0)
            {
                foreach (GameObject g in hittedmeshes)
                {
                    slice(trailer.getList(), g);
                }
            }
            else
            {
                print("Bad trail!");
            }
        }
    }

    private void setTrailFirsts(List<Vector3> trail, GameObject g)
    {
        firstIN = Vector3.zero;
        secondIN = Vector3.zero;
        firstOUT = Vector3.zero;
        secondOUT = Vector3.zero;

        bool firstFound = false, lastFound = false;
        for (int i = 0; i < trail.Count; i++)
        {
            GameObject tmp = insideMesh(trail[i]);
            if (firstFound == false)
            {
                if (tmp != null && g.GetInstanceID() == tmp.GetInstanceID())
                {
                    firstFound = true;
                    firstIN = trail[i];
                    firstOUT = trail[i - 1];
                }
            }
            else if (lastFound == false)
            {
                if (tmp == null || g.GetInstanceID() != tmp.GetInstanceID())
                {
                    lastFound = true;
                    secondOUT = trail[i];
                    secondIN = trail[i - 1];
                }
            }
        }
    }



    private void slice(List<Vector3> trail, GameObject g)
    {
        GameObject toBeSliced = g;
        Vector3 closest = Vector3.zero;
        Vector3 closest2 = Vector3.zero;
        Vector3 closest3 = Vector3.zero;
        Vector3 closest4 = Vector3.zero;

        List<Vector3> vectorsRight = new List<Vector3>();
        List<Vector3> vectorsLeft = new List<Vector3>();
        List<Vector2> uvsRight = new List<Vector2>();
        List<Vector2> uvsLeft = new List<Vector2>();

        //Initial stuff------------------
        //Mesh stuffz
        MeshFilter mf = toBeSliced.GetComponent<MeshFilter>();
        Mesh tmpMesh = new Mesh();

        //Original meshes stuffz
        Vector3[] origVerts = mf.sharedMesh.vertices;
        Vector2[] origUvs = mf.sharedMesh.uv;
        int[] origTris = mf.sharedMesh.triangles;
        Vector3[] origNorms = mf.sharedMesh.normals;

        //Trail vectors
        List<Vector3> trailVs = trailer.getList();
        closest = Vector3.zero;
        closest2 = Vector3.zero;
        closest3 = Vector3.zero;
        closest4 = Vector3.zero;
        //------------------------------

        setTrailFirsts(trailVs, toBeSliced);

        //Finding the closest points----------------------------------------------------------------------------------
        //Find the closest points in both sides of the trails penetrating points
        for (int i = 0; i < origVerts.Length; i++)
        {
            Vector3 tmp = origVerts[i];
            if (closest == Vector3.zero && closest3 == Vector3.zero)
            {
                closest = tmp;
                closest3 = tmp;
            }
            else if (closest2 == Vector3.zero && closest4 == Vector3.zero)
            {
                closest2 = tmp;
                closest4 = tmp;
            }
            else
            {
                float closestDist = Vector3.Distance(closest, firstOUT) + Vector3.Distance(closest2, firstOUT);
                float tmpDist = Vector3.Distance(origVerts[i], firstOUT) + Vector3.Distance(origVerts[i - 1], firstOUT);

                float closestDist2 = Vector3.Distance(closest3, secondOUT) + Vector3.Distance(closest4, secondOUT);
                float tmpDist2 = Vector3.Distance(origVerts[i], secondOUT) + Vector3.Distance(origVerts[i - 1], secondOUT);

                if (closestDist > tmpDist)
                {
                    closest = origVerts[i];
                    closest2 = origVerts[i - 1];
                }

                if (closestDist2 > tmpDist2)
                {
                    closest3 = origVerts[i];
                    closest4 = origVerts[i - 1];
                }
            }
        }
        //-------------------------------------------------------------------------------
        //Set the impact points for the in and out points
        impactPointIN = Vector3.zero;
        impactPointOUT = Vector3.zero;

        impactPointIN = LineIntersectionPoint(closest, closest2, firstOUT, firstIN);
        impactPointOUT = LineIntersectionPoint(closest3, closest4, secondIN, secondOUT);

        //-----------------------------------------------------------------------------------
        //Setting the vectors and uvs---------------------------------------------------
        //Goes throught the vectors and puts them in dynamic lists for the right and left side

        List<List<Vector3>> VlistList = new List<List<Vector3>>();
        List<List<Vector2>> UVlistList = new List<List<Vector2>>();
        List<Vector3> closests = new List<Vector3>();
        closests.Add(closest);
        closests.Add(closest3);
        VlistList.Add(vectorsRight);
        VlistList.Add(vectorsLeft);
        UVlistList.Add(uvsRight);
        UVlistList.Add(uvsLeft);

        int rightSide = 0;
        int leftSide = 1;
        int listIndice = 0;
        bool added = false;
        foreach (Vector3 origV in origVerts)
        {
            //Center point is not carried to the new meshes
            if (isNotCenterPoint(origV))
            {
                //Indicates the array that the vertice should be added to

                //Checks if the currect vector is either of the closest ones and sets the indice to correct side
                foreach (Vector3 cV in closests)
                {
                    if (cV == origV)
                    {
                        if (listIndice == leftSide)
                        {
                            listIndice = rightSide;
                        }
                        else
                        {
                            listIndice = leftSide;
                        }

                        if (cV == closest && added == false)
                        {
                            added = true;
                            addVertice(vectorsRight, impactPointIN, uvsRight);
                            for (int i = 0; i < trailVs.Count; i++)
                            {
                                if (insideMesh(trailVs[i], toBeSliced))
                                {
                                    addVertice(vectorsRight, trailVs[i], uvsRight);
                                }
                            }
                            addVertice(vectorsRight, impactPointOUT, uvsRight);

                            addVertice(vectorsLeft, impactPointOUT, uvsLeft);
                            for (int i = trailVs.Count - 1; i >= 0; i--)
                            {
                                if (insideMesh(trailVs[i], toBeSliced))
                                {
                                    addVertice(vectorsLeft, trailVs[i], uvsLeft);
                                }
                            }
                            addVertice(vectorsLeft, impactPointIN, uvsLeft);
                        }
                        else if (added == false)
                        {
                            added = true;
                            addVertice(vectorsRight, impactPointOUT, uvsRight);
                            for (int i = trailVs.Count - 1; i >= 0; i--)
                            {
                                if (insideMesh(trailVs[i], toBeSliced))
                                {
                                    addVertice(vectorsRight, trailVs[i], uvsRight);
                                }
                            }
                            addVertice(vectorsRight, impactPointIN, uvsRight);

                            addVertice(vectorsLeft, impactPointIN, uvsLeft);
                            for (int i = 0; i < trailVs.Count; i++)
                            {
                                if (insideMesh(trailVs[i], toBeSliced))
                                {
                                    addVertice(vectorsLeft, trailVs[i], uvsLeft);
                                }
                            }
                            addVertice(vectorsLeft, impactPointOUT, uvsLeft);
                        }
                    }
                }
                addVertice(VlistList[listIndice], origV, UVlistList[listIndice]);

            }
        }
        //Calculate center points of the meshes and put them as the firsts in the lists
        //print("LeftCount: " + vectorsLeft.Count);
        Vector3 lCenterV = calculateCenter(vectorsLeft);
        vectorsLeft[0] = lCenterV;
        float normedHorizontalL = (lCenterV.x + 1.0f) * 0.5f;
        float normedVerticalL = (lCenterV.y + 1.0f) * 0.5f;
        uvsLeft[0] = new Vector2(normedHorizontalL, normedVerticalL);
        //print("LeftCount: " + vectorsLeft.Count);

        //print("RightCount: " + vectorsRight.Count);
        Vector3 rCenterV = calculateCenter(vectorsRight);
        vectorsRight[0] = rCenterV;
        float normedHorizontalR = (rCenterV.x + 1.0f) * 0.5f;
        float normedVerticalR = (rCenterV.y + 1.0f) * 0.5f;
        uvsRight[0] = new Vector2(normedHorizontalR, normedVerticalR);
        //--------------------------------------------------------------------------------------------------


        //Triangle setting part-------------------------------------------------------------------------------
        //New stuff
        int[] rightTris = new int[vectorsRight.Count * 3];
        //Iterate through the all pie slices.
        for (int i = 0; i + 2 < vectorsRight.Count; ++i)
        {
            //Index is the place where the iteration should start with the tris. Every iteration places three indexes to the tris array.
            int index = i * 3;
            //Triangles first point is always the center
            rightTris[index + 0] = 0;
            //Second point is the next in the array
            rightTris[index + 1] = i + 1;
            //And third is still next
            rightTris[index + 2] = i + 2;
        }

        // The last triangle has to wrap around to the first vert so we do this last and outside the loop
        //Index for the first vert of the last tris
        int lastTriangleIndex = rightTris.Length - 3;
        //First vert is always center
        rightTris[lastTriangleIndex + 0] = 0;
        //Second is the last vertice
        rightTris[lastTriangleIndex + 1] = vectorsRight.Count - 1;
        //third is the first vertice
        rightTris[lastTriangleIndex + 2] = 1;



        //----------------------------------------------------------------------------------------
        //Second tris----------------------------
        int[] leftTris = new int[vectorsLeft.Count * 3];
        //Iterate through the all pie slices.
        for (int i = 0; i + 2 < vectorsLeft.Count; ++i)
        {
            //Index is the place where the iteration should start with the tris. Every iteration places three indexes to the tris array.
            int index = i * 3;
            //Triangles first point is always the center
            leftTris[index + 0] = 0;
            //Second point is the next in the array
            leftTris[index + 1] = i + 1;
            //And third is still next
            leftTris[index + 2] = i + 2;
        }

        // The last triangle has to wrap around to the first vert so we do this last and outside the loop
        //Index for the first vert of the last tris
        lastTriangleIndex = leftTris.Length - 3;
        //First vert is always center
        leftTris[lastTriangleIndex + 0] = 0;
        //Second is the last vertice
        leftTris[lastTriangleIndex + 1] = vectorsLeft.Count - 1;
        //third is the first vertice
        leftTris[lastTriangleIndex + 2] = 1;

        //Create the mesh and assing it to the object-------------------------------------------------------
        float area1 = 0, area2 = 0;
        Vector3[] planeVerts = new Vector3[vectorsRight.Count];
        Vector2[] planeUVs = new Vector2[uvsRight.Count];
        int[] planeTris = new int[rightTris.Length];

        vectorsRight.CopyTo(planeVerts);
        uvsRight.CopyTo(planeUVs);
        rightTris.CopyTo(planeTris, 0);
        //print("first UV length: " + uvsRight.Count + " , vert Length: " + vectorsRight.Count);
        Mesh plane = new Mesh();
        plane.vertices = planeVerts;
        plane.triangles = planeTris;
        plane.uv = planeUVs;

        //print("first UV length: " + plane.uv.Length + " , vert Length: " + plane.vertices.Length);

        //Recalculate normals. Would it work without adding them in code at all?
        plane.RecalculateNormals();

        area1 = calculaterMeshArea(plane);

        toBeSliced.GetComponent<MeshFilter>().sharedMesh = plane;
        DestroyImmediate(toBeSliced.GetComponent<MeshCollider>());
        toBeSliced.AddComponent<MeshCollider>();
        //--------------------------------------------------------------------------------------------
        //Second mesh
        planeVerts = new Vector3[vectorsLeft.Count];
        planeUVs = new Vector2[uvsLeft.Count];
        planeTris = new int[leftTris.Length];

        vectorsLeft.CopyTo(planeVerts);
        uvsLeft.CopyTo(planeUVs);
        leftTris.CopyTo(planeTris, 0);

        plane = new Mesh();
        plane.vertices = planeVerts;
        plane.triangles = planeTris;
        plane.uv = planeUVs;

        plane.RecalculateNormals();

        area2 = calculaterMeshArea(plane);

        GameObject newMesh = (GameObject)Instantiate(meshPref, new Vector3(0, 0, 0), Quaternion.identity);
        newMesh.GetComponent<MeshFilter>().sharedMesh = plane;
        newMesh.AddComponent<MeshCollider>();
        meshHandler.addMesh(newMesh);
        newMesh.transform.parent = meshParent.transform;
        Vector3 newPos = Vector3.zero;
        if (area2 < area1)
        {
            newPos = newMesh.transform.position;
            newPos.z = 4;
            newMesh.transform.position = newPos;
        }
        else
        {
            newPos = toBeSliced.transform.position;
            newPos.z = 4;
            toBeSliced.transform.position = newPos;
        }
    }





    private bool insideMesh(Vector3 v, GameObject g)
    {
        Vector3 direction = v;
        direction.z = 1;
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(transform.position, direction, out hit))
        {
            if (hit.transform.gameObject.GetInstanceID() == g.GetInstanceID())
            {
                return true;
            }

        }
        return false;
    }


    private GameObject insideMesh(Vector3 v)
    {
        Vector3 direction = v;
        direction.z = 1;
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(transform.position, direction, out hit))
        {
            return hit.transform.gameObject;
        }
        return null;
    }

    private List<GameObject> getHittedMeshes(List<Vector3> trail)
    {
        List<GameObject> hitted = new List<GameObject>();

        firstIN = Vector3.zero;
        secondIN = Vector3.zero;
        firstOUT = Vector3.zero;
        secondOUT = Vector3.zero;

        inIndex = 0;
        outIndex = 0;

        bool goneIn = false;
        bool goneOut = false;
        for (int i = 0; i < trail.Count; i++)
        {
            GameObject g = insideMesh(trail[i]);

            bool isInside = false;
            if (g != null)
            {
                isInside = true;
                if (!hitted.Contains(g))
                {
                    hitted.Add(g);
                }
            }



            if (goneIn == false)
            {
                if (isInside == true)
                {
                    firstIN = trail[i];
                    inIndex = i;
                    if (i - 1 >= 0)
                    {
                        firstOUT = trail[i - 1];
                    }
                    else
                    {
                        return null;
                    }

                    goneIn = true;
                }
            }
            else if (goneOut == false)
            {
                if (isInside == false)
                {
                    secondOUT = trail[i];
                    outIndex = i;
                    if (i - 1 >= 0)
                    {
                        secondIN = trail[i - 1];
                    }
                    else
                    {
                        return null;
                    }

                    goneOut = true;
                }
            }
            else
            {
                if (isInside)
                {
                    return null;
                }
            }

        }
        if (goneIn == true && goneOut == true)
        {
            return hitted;
        }
        return null;
    }

    private bool isNotCenterPoint(Vector3 v)
    {
        foreach (Vector3 c in centerPoints)
        {
            if (v == c)
            {
                return false;
            }
        }
        return true;
    }

    Vector2 LineIntersectionPoint(Vector3 ps1, Vector3 pe1, Vector3 ps2,
   Vector2 pe2)
    {
        // Get A,B,C of first line - points : ps1 to pe1
        float A1 = pe1.y - ps1.y;
        float B1 = ps1.x - pe1.x;
        float C1 = A1 * ps1.x + B1 * ps1.y;

        // Get A,B,C of second line - points : ps2 to pe2
        float A2 = pe2.y - ps2.y;
        float B2 = ps2.x - pe2.x;
        float C2 = A2 * ps2.x + B2 * ps2.y;

        // Get delta and check if the lines are parallel
        float delta = A1 * B2 - A2 * B1;
        if (delta == 0)
            throw new System.Exception("Lines are parallel");

        // now return the Vector2 intersection point
        return new Vector3(
            (B2 * C1 - B1 * C2) / delta,
            (A1 * C2 - A2 * C1) / delta,
            ps1.z
        );
    }



    private float calculaterMeshArea(Mesh m)
    {
        Vector3[] vertices = m.vertices;
        int[] triangles = m.triangles;
        float areaSum = 0;
        for (int i = 0; i + 2 < triangles.Length; i += 3)
        {
            float length1, length2, length3, s;
            length1 = Vector3.Distance(vertices[triangles[i]], vertices[triangles[i + 1]]);
            length2 = Vector3.Distance(vertices[triangles[i + 1]], vertices[triangles[i + 2]]);
            length3 = Vector3.Distance(vertices[triangles[i + 2]], vertices[triangles[i]]);
            s = (length1 + length2 + length3) / 2;
            float area = 0;
            area += Mathf.Sqrt(s * (s - length1) * (s - length2) * (s - length3));
            areaSum += area;
        }
        return areaSum;
    }

    private void addVertice(List<Vector3> list, Vector3 v, List<Vector2> uvs)
    {
        list.Add(v);
        float normedHorizontal = (v.x + 1.0f) * 0.5f;
        float normedVertical = (v.y + 1.0f) * 0.5f;
        uvs.Add(new Vector2(normedHorizontal, normedVertical));
    }

    private Vector3 calculateCenter(List<Vector3> vList)
    {
        int count = 0;
        Vector3 average = Vector3.zero;
        foreach (Vector3 v in vList)
        {
            count++;
            average += v;
        }
        average = average / count;
        return average;
    }
}
