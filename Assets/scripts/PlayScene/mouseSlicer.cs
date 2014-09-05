using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Cutted parts can be found again by comparing the current vertices to the cut trail points. Both current area and the deleted area can be combined by replacing the points in trail with the rest from the deleted piece

public class mouseSlicer : MonoBehaviour
{

    private LayerMask lMask = 1 << 0;

    public GameObject cutArea;
    public SliceTrailer trailer;
    public LineRenderer liner;
    public GameObject meshParent;
    public GameObject meshPref;
    public MeshHandler2 meshHandler;
    public SceneScript sceneHandler;

    public LiquidScript liquider;

    private Vector3 firstOUT;
    private Vector3 firstIN;
    private Vector3 secondIN;
    private Vector3 secondOUT;

    private List<Vector3> debugTrailList = new List<Vector3>();
    private List<Vector3> debugFirstMeshList = new List<Vector3>();
    private List<Vector3> debugSecondMeshList = new List<Vector3>();

    public GameObject enemyParent;

    private Vector3 impactPointIN = Vector3.zero, impactPointOUT = Vector3.zero;

    List<Vector3> centerPoints = new List<Vector3>();

    private bool drawDebug = true;

    bool onMobile = false;

    // Use this for initialization
    void Start()
    {
        this.sceneHandler = GameObject.Find("SceneScript").GetComponent<SceneScript>();
        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            onMobile = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (centerPoints.Count == 0)
        {
            centerPoints.Add(cutArea.GetComponent<MeshFilter>().sharedMesh.vertices[0]);
        }
        if (onMobile)
        {
            handleTouch();
        }
        else
        {
            handleMouse();
        }

        if (drawDebug)
        {
            DrawDebug();
        }
        if (trailer.getList() != null)
        {
            if (checkEnemyCollision(trailer.getList()))
            {
                trailer.endTrail(Input.mousePosition);
                liner.SetVertexCount(0);
                trailer.startTrail(Input.mousePosition);
            }
        }
    }

    private void DrawDebug()
    {
        foreach (Vector3 v in debugTrailList)
        {
            Debug.DrawLine(v, new Vector3(15, 0, 0), Color.red);
        }
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

    private bool checkEnemyCollision(List<Vector3> pointsList)
    {
        int i = 0;
        foreach (Vector3 v in pointsList)
        {
            i++;
            if (pointsList.Count > i)
            {
                Vector3 v2 = pointsList[i];
                RaycastHit2D hit = Physics2D.Raycast(v, v2, Vector3.Distance(v, v2));
                Debug.DrawLine(v, v2, Color.green);
                if (hit.transform != null)
                {
                    print(hit.transform.parent.gameObject.tag);
                    if (hit.transform.parent.gameObject.tag == ("BigEnemy"))
                    {
                        debugTrailList.Add(hit.transform.position);
                        //sceneHandler.EndGame();
                    }
                    if (hit.transform.gameObject.tag != "OuterRing")
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void handleTouch()
    {
        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
            trailer.startTrail(Input.mousePosition);
        }
        if (Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            trailer.Add(Input.mousePosition);
            drawLine();

        }

        if (Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            liner.SetVertexCount(0);
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

    private void handleMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            trailer.startTrail(Input.mousePosition);
        }
        if (Input.GetMouseButton(0))
        {

            trailer.Add(Input.mousePosition);
            drawLine();

        }

        if (Input.GetMouseButtonUp(0))
        {
            liner.SetVertexCount(0);
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

        //Original meshes stuffz
        Vector3[] origVerts = mf.sharedMesh.vertices;

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

        //impactPointIN = LineIntersectionPoint(closest, closest2, firstOUT, firstIN);
        //impactPointOUT = LineIntersectionPoint(closest3, closest4, secondIN, secondOUT);

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
                bool addVector = true;
                //Indicates the array that the vertice should be added to
                //Checks if the currect vector is either of the closest ones and sets the indice to correct side
                foreach (Vector3 cV in closests)
                {
                    if (cV == origV)
                    {
                        addVector = false;
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
                            //addVertice(vectorsRight, impactPointIN, uvsRight);
                            for (int i = 0; i < trailVs.Count; i++)
                            {
                                if (insideMesh(trailVs[i], toBeSliced))
                                {
                                    addVertice(vectorsRight, trailVs[i], uvsRight);
                                }
                            }
                            //addVertice(vectorsRight, impactPointOUT, uvsRight);

                            //addVertice(vectorsLeft, impactPointOUT, uvsLeft);
                            for (int i = trailVs.Count - 1; i >= 0; i--)
                            {
                                if (insideMesh(trailVs[i], toBeSliced))
                                {
                                    addVertice(vectorsLeft, trailVs[i], uvsLeft);
                                }
                            }
                            //addVertice(vectorsLeft, impactPointIN, uvsLeft);
                        }
                        else if (added == false)
                        {
                            added = true;
                            //addVertice(vectorsRight, impactPointOUT, uvsRight);
                            for (int i = trailVs.Count - 1; i >= 0; i--)
                            {
                                if (insideMesh(trailVs[i], toBeSliced))
                                {
                                    addVertice(vectorsRight, trailVs[i], uvsRight);
                                }
                            }
                            //addVertice(vectorsRight, impactPointIN, uvsRight);

                            //addVertice(vectorsLeft, impactPointIN, uvsLeft);
                            for (int i = 0; i < trailVs.Count; i++)
                            {
                                if (insideMesh(trailVs[i], toBeSliced))
                                {
                                    addVertice(vectorsLeft, trailVs[i], uvsLeft);
                                }
                            }
                            //addVertice(vectorsLeft, impactPointOUT, uvsLeft);
                        }
                    }
                }
                if (addVector)
                {
                    addVertice(VlistList[listIndice], origV, UVlistList[listIndice]);
                }
            }
        }
        //Calculate center points of the meshes and put them as the firsts in the lists
        //print("LeftCount: " + vectorsLeft.Count);
        Vector3 lCenterV = calculateCenter(vectorsLeft);
        vectorsLeft.Insert(0, lCenterV);
        //vectorsLeft[0] = lCenterV;
        float normedHorizontalL = (lCenterV.x + 1.0f) * 0.5f;
        float normedVerticalL = (lCenterV.y + 1.0f) * 0.5f;
        //uvsLeft[0] = new Vector2(normedHorizontalL, normedVerticalL);
        uvsLeft.Insert(0, new Vector2(normedHorizontalL, normedVerticalL));
        //print("LeftCount: " + vectorsLeft.Count);

        //print("RightCount: " + vectorsRight.Count);
        Vector3 rCenterV = calculateCenter(vectorsRight);
        vectorsRight.Insert(0, rCenterV);
        //vectorsRight[0] = rCenterV;
        float normedHorizontalR = (rCenterV.x + 1.0f) * 0.5f;
        float normedVerticalR = (rCenterV.y + 1.0f) * 0.5f;
        //uvsRight[0] = new Vector2(normedHorizontalR, normedVerticalR);
        uvsRight.Insert(0, new Vector2(normedHorizontalR, normedVerticalR));

        centerPoints.Add(vectorsRight[0]);
        centerPoints.Add(vectorsLeft[0]);

        //--------------------------------------------------------------------------------------------------

        debugTrailList = new List<Vector3>();
        //Triangle setting part-------------------------------------------------------------------------------
        //New stuff
        /*int[] rightTris = new int[vectorsRight.Count * 3];
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
        debugTrailList.Add(vectorsRight[rightTris[lastTriangleIndex + 0]]);
        debugTrailList.Add(vectorsRight[rightTris[lastTriangleIndex + 1]]);
        debugTrailList.Add(vectorsRight[rightTris[lastTriangleIndex + 2]]);*/

        int[] rightTris = new int[vectorsRight.Count * 3];
        //Iterate through the all pie slices.
        bool allDone = false;
        int left = 0;
        int right = vectorsRight.Count - 1;
        int ind = 0;
        //Here set the first tris.
        left++;
        bool leftBig = true;
        while (!allDone)
        {
            //print("left: " + left + " , right: " + right);
            if (right != left)
            {
                int index = ind * 3;
                //Triangles first point is always the center
                rightTris[index + 0] = right;
                //Second point is the next in the array
                rightTris[index + 1] = left;
                //And third is still next
                if (leftBig)
                {
                    left++;
                    rightTris[index + 2] = left;
                    leftBig = false;
                }
                else
                {
                    right--;
                    rightTris[index + 2] = right;
                    leftBig = true;
                }
            }
            else
            {
                allDone = true;
            }
            ind++;
        }


        //----------------------------------------------------------------------------------------



























        //Second tris----------------------------
        /*int[] leftTris = new int[vectorsLeft.Count * 3];
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
        leftTris[lastTriangleIndex + 2] = 1;*/


        int[] leftTris = new int[vectorsLeft.Count * 3];
        //Iterate through the all pie slices.
        allDone = false;
        left = 0;
        right = vectorsLeft.Count - 1;
        ind = 0;
        //Here set the first tris.
        left++;
        leftBig = true;
        while (!allDone)
        {
            //print("left: " + left + " , right: " + right);
            if (right != left)
            {
                //print("new Polygon-------------------------------");
                int index = ind * 3;
                //Triangles first point is always the center
                leftTris[index + 0] = right;
                //print(vectorsLeft[leftTris[index + 0]]);
                //Second point is the next in the array
                leftTris[index + 1] = left;
                //print(vectorsLeft[leftTris[index + 1]]);
                //And third is still next
                if (leftBig)
                {
                    left++;
                    leftTris[index + 2] = left;
                    //print(vectorsLeft[leftTris[index + 2]]);
                    leftBig = false;
                }
                else
                {
                    right--;
                    leftTris[index + 2] = right;
                    //print(vectorsLeft[leftTris[index + 2]]);
                    leftBig = true;
                }
            }
            else
            {
                allDone = true;
            }
            ind++;
        }











        //debugTrailList.Add(vectorsLeft[leftTris[lastTriangleIndex + 0]]);
        //debugTrailList.Add(vectorsLeft[leftTris[lastTriangleIndex + 1]]);
        //debugTrailList.Add(vectorsLeft[leftTris[lastTriangleIndex + 2]]);

        //Create the mesh and assing it to the object-------------------------------------------------------
        float area1 = 0, area2 = 0;
        Vector3[] planeVerts = new Vector3[vectorsRight.Count];
        Vector2[] planeUVs = new Vector2[uvsRight.Count];
        int[] planeTris = new int[rightTris.Length];

        vectorsRight.CopyTo(planeVerts);
        debugFirstMeshList = vectorsRight;
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

        /*toBeSliced.GetComponent<MeshFilter>().sharedMesh.vertices = planeVerts;
        toBeSliced.GetComponent<MeshFilter>().sharedMesh.triangles = planeTris;
        toBeSliced.GetComponent<MeshFilter>().sharedMesh.uv = planeUVs;*/

        DestroyImmediate(toBeSliced.GetComponent<MeshCollider>());
        toBeSliced.AddComponent<MeshCollider>();
        toBeSliced.GetComponent<MeshCollider>().isTrigger = true;
        //--------------------------------------------------------------------------------------------
        //Second mesh
        planeVerts = new Vector3[vectorsLeft.Count];
        planeUVs = new Vector2[uvsLeft.Count];
        planeTris = new int[leftTris.Length];

        vectorsLeft.CopyTo(planeVerts);
        debugSecondMeshList = vectorsLeft;
        uvsLeft.CopyTo(planeUVs);
        leftTris.CopyTo(planeTris, 0);

        plane = new Mesh();
        plane.vertices = planeVerts;
        plane.triangles = planeTris;
        plane.uv = planeUVs;

        plane.RecalculateNormals();

        area2 = calculaterMeshArea(plane);

        GameObject newMesh = (GameObject)Instantiate(meshPref, new Vector3(0, 0, 0), Quaternion.identity);
        newMesh.transform.localPosition = new Vector3(0, 0, 0);
        newMesh.GetComponent<MeshFilter>().sharedMesh = plane;

        newMesh.AddComponent<MeshCollider>();
        //meshHandler.addMesh(newMesh);
        newMesh.transform.parent = meshParent.transform;
        meshHandler.meshCutted();
        Vector3 newPos = Vector3.zero;
        if (area2 < area1)
        {
            newPos = newMesh.transform.position;
            newPos.z = 4;
            newMesh.transform.position = newPos;
            Destroy(newMesh);
            liquider.addLiquid(area2);
        }
        else
        {
            newPos = toBeSliced.transform.position;
            newPos.z = 4;
            toBeSliced.transform.position = newPos;
            Destroy(toBeSliced);
            liquider.addLiquid(area1);
        }
    }

    private bool insideMesh(Vector3 v, GameObject g)
    {
        Vector3 direction = v;
        direction.z = 1;
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity, lMask))
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
        if (Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity, lMask))
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
        Vector3 average = Vector3.zero;
        int count = 0;
        foreach (Vector3 v in vList)
        {
            count++;
            average += v;
        }
        average = average / count;
        return average;
    }
}
