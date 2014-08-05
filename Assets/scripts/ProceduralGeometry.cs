using UnityEngine;
using System.Collections;

// =============================================================================
public class ProceduralGeometry
{
    static Vector2 vect = new Vector2(0, 5);
    // -------------------------------------------------------------------------
    static public Mesh CreateCircle(float radius)
    {

        //How many vertices is wanted
        int numVerts = 41;

        //Mesh where the generated mesh is created
        Mesh plane = new Mesh();
        //Vertices for the circle
        Vector3[] verts = new Vector3[numVerts];
        //UVs of the mesh
        Vector2[] uvs = new Vector2[numVerts];
        //Triangles of the mesh
        int[] tris = new int[(numVerts * 3)];

        //First Vertice is the zero point where all the triangles are created from
        verts[0] = Vector3.zero;
        //UV is 0.5,0.5 for some reason
        uvs[0] = new Vector2(0.5f, 0.5f);
        //How much the vector is rotated each time. Calculated to be some part of 360 degrees
        float angle = 360.0f / (float)(numVerts - 1);



        //----------------------------------

        //To get an ellipse i count just add and decrease the length of the vector somehow. Vector is distance also.
        for (int i = 1; i < numVerts; ++i)
        {
            //calculate how much this piece should be rotated
            float rotateAmount = angle * (float)i - 1;

            //Calculate a Quaternion from it. Used in next line.
            Quaternion quaternion = Quaternion.AngleAxis(rotateAmount, Vector3.back);
            //Places the Vertice. Vertice is Vector3.up rotated by the amout dictated by the quaternion variable
            verts[i] = quaternion * vect;

            //Calculating some normalized floats for the UV
            float normedHorizontal = (verts[i].x + 1.0f) * 0.5f;
            float normedVertical = (verts[i].y + 1.0f) * 0.5f;
            //Assinging the UV
            uvs[i] = new Vector2(normedHorizontal, normedVertical);
        }

        //Iterate through the all pie slices.
        for (int i = 0; i + 2 < numVerts; ++i)
        {
            //Index is the place where the iteration should start with the tris. Every iteration places three indexes to the tris array.
            int index = i * 3;
            //Triangles first point is always the center
            tris[index + 0] = 0;
            //Second point is the next in the array
            tris[index + 1] = i + 1;
            //And third is still next
            tris[index + 2] = i + 2;
        }

        // The last triangle has to wrap around to the first vert so we do this last and outside the loop

        //Index for the first vert of the last tris
        int lastTriangleIndex = tris.Length - 3;
        //First vert is always center
        tris[lastTriangleIndex + 0] = 0;
        //Second is the last vertice
        tris[lastTriangleIndex + 1] = numVerts - 1;
        //third is the first vertice
        tris[lastTriangleIndex + 2] = 1;

        //Set the planes vertices, triangles and UVs (mine didnt work because i forgot this)
        plane.vertices = verts;
        plane.triangles = tris;
        plane.uv = uvs;

        //Recalculate normals. Would it work without adding them in code at all?
        plane.RecalculateNormals();

        //Return the done plane for the caller
        return plane;
    }
}
