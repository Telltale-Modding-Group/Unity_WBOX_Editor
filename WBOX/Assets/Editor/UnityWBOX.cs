#if UNITY_EDITOR

using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.AI;
using UnityEngine.AI;

namespace UnityTelltaleTools
{
    public class UnityWBOX : EditorWindow
    {
        //GUI related
        private int tabIndex;
        private static int guiSectionSpacePixels = 10;
        private static string[] tabNames = new string[] { "Import", "Generate", "OBJ" };

        private WalkBoxes_Master walkboxes_master;

        //add a menu item at the top of the unity editor toolbar
        [MenuItem("Telltale/WBOX Tools")]
        public static void ShowWindow()
        {
            //get the window and open it
            GetWindow(typeof(UnityWBOX));
        }

        /// <summary>
        /// GUI display function for the window
        /// </summary>
        void OnGUI()
        {
            //window title
            GUILayout.Label("WBOX Tools", EditorStyles.whiteLargeLabel);

            //create a toolbar to organize our mess
            tabIndex = GUILayout.Toolbar(tabIndex, tabNames);

            if(tabIndex == 0)
            {
                //section title
                GUILayout.Label("[Import .wbox]", EditorStyles.boldLabel);
                GUILayout.Space(guiSectionSpacePixels);

                if (GUILayout.Button("Import Walkbox"))
                {
                    ImportUnityWBOX();
                }
            }
            else if(tabIndex == 1)
            {
                //section title
                GUILayout.Label("[Generate .wbox]", EditorStyles.boldLabel);
                GUILayout.Space(guiSectionSpacePixels);

                if (GUILayout.Button("Generate Walkbox From NavMesh"))
                {
                    GenerateUnityWBOX();
                }
            }
            else if (tabIndex == 2)
            {
                //section title
                GUILayout.Label("[Convert .wbox to .obj]", EditorStyles.boldLabel);
                GUILayout.Space(guiSectionSpacePixels);

                if (GUILayout.Button("Convert Walkbox To OBJ"))
                {
                    ImportWBOX_ToOBJ();
                }
            }
        }

        public void ImportWBOX_ToOBJ()
        {
            string filePath = EditorUtility.OpenFilePanel("Import Walkbox To Convert To Obj", "", "wbox");

            walkboxes_master = new WalkBoxes_Master(filePath);
            Walkboxes walkboxes = walkboxes_master.walkboxes;

            //-----------------------------------------
            //mVerts

            List<UnityEngine.Vector3> meshVerticies = new List<UnityEngine.Vector3>();

            for (int i = 0; i < walkboxes.mVerts.Length; i++)
            {
                Vert mVert = walkboxes.mVerts[i];

                UnityEngine.Vector3 unityVertexPosition = new UnityEngine.Vector3()
                {
                    x = mVert.mPos.x,
                    y = mVert.mPos.y,
                    z = mVert.mPos.z
                };

                meshVerticies.Add(unityVertexPosition);
            }

            //-----------------------------------------
            //mNormals

            List<UnityEngine.Vector3> meshNormals = new List<UnityEngine.Vector3>();

            for (int i = 0; i < walkboxes.mNormals.Length; i++)
            {
                Vector3 mNormal = walkboxes.mNormals[i];

                UnityEngine.Vector3 unity_normal = new UnityEngine.Vector3()
                {
                    x = mNormal.x,
                    y = mNormal.y,
                    z = mNormal.z
                };

                meshNormals.Add(unity_normal);
            }

            //-----------------------------------------
            //mQuads
            for (int i = 0; i < walkboxes.mQuads.Length; i++)
            {
                Quad mQuad = walkboxes.mQuads[i];
            }

            //-----------------------------------------
            //mTris

            List<int> meshTriangles = new List<int>();

            for (int i = 0; i < walkboxes.mTris.Length; i++)
            {
                Tri mTri = walkboxes.mTris[i];

                for (int x = 0; x < mTri.mEdgeInfo.Length; x++) //SArray<WalkBoxes::Edge,3>
                {
                    //-----------------------------------------
                    //mEdgeInfo
                    Edge mEdge = mTri.mEdgeInfo[x];
                }

                meshTriangles.Add(mTri.mVerts[0]);
                meshTriangles.Add(mTri.mVerts[1]);
                meshTriangles.Add(mTri.mVerts[2]);
            }

            Mesh mesh = new Mesh();
            mesh.name = walkboxes.mName;
            mesh.vertices = meshVerticies.ToArray();
            mesh.triangles = meshTriangles.ToArray();
            mesh.normals = meshNormals.ToArray();

            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
            mesh.OptimizeIndexBuffers();
            mesh.OptimizeReorderVertexBuffer();
            mesh.Optimize();

            string finalPath = Application.dataPath + "/" + walkboxes.mName + ".obj";

            using (StreamWriter sw = new StreamWriter(finalPath))
            {
                sw.Write(MeshToString(mesh));
            }

            AssetDatabase.Refresh();

            EditorUtility.RevealInFinder(finalPath);
        }

        private static string MeshToString(Mesh mesh)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("g ").Append(mesh.name).Append("\n");

            foreach (UnityEngine.Vector3 v in mesh.vertices)
            {
                sb.Append(string.Format("v {0} {1} {2}\n", v.x, v.y, v.z));
            }

            sb.Append("\n");

            foreach (UnityEngine.Vector3 v in mesh.normals)
            {
                sb.Append(string.Format("vn {0} {1} {2}\n", v.x, v.y, v.z));
            }

            sb.Append("\n");

            foreach (UnityEngine.Vector3 v in mesh.uv)
            {
                sb.Append(string.Format("vt {0} {1}\n", v.x, v.y));
            }

            for (int material = 0; material < mesh.subMeshCount; material++)
            {
                sb.Append("\n");
                //sb.Append("usemtl ").Append(mats[material].name).Append("\n");
                //sb.Append("usemap ").Append(mats[material].name).Append("\n");

                int[] triangles = mesh.GetTriangles(material);

                for (int i = 0; i < triangles.Length; i += 3)
                {
                    sb.Append(string.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n", triangles[i] + 1, triangles[i + 1] + 1, triangles[i + 2] + 1));
                }
            }

            return sb.ToString();
        }

        private void ImportUnityWBOX()
        {
            string filePath = EditorUtility.OpenFilePanel("Import Walkbox", "", "wbox");

            walkboxes_master = new WalkBoxes_Master(filePath);
            Walkboxes walkboxes = walkboxes_master.walkboxes;

            GameObject wbox_parent_gameObject = new GameObject(walkboxes.mName);
            ProxyWalkboxes proxy_wbox = wbox_parent_gameObject.AddComponent<ProxyWalkboxes>();

            proxy_wbox.msv6 = walkboxes_master.msv6;
            proxy_wbox.msv5 = walkboxes_master.msv5;
            proxy_wbox.mtre = walkboxes_master.mtre;

            //-----------------------------------------
            //mVerts

            proxy_wbox.mVerts = new List<ProxyVert>();

            GameObject wbox_parent_mVerts_gameObject = new GameObject("mVerts");
            wbox_parent_mVerts_gameObject.transform.SetParent(wbox_parent_gameObject.transform);
            proxy_wbox.mVerts_parent = wbox_parent_mVerts_gameObject;

            for (int i = 0; i < walkboxes.mVerts.Length; i++)
            {
                GameObject wbox_vert_gameObject = new GameObject(string.Format("mVert {0}", i));
                ProxyVert proxy_mVert = wbox_vert_gameObject.AddComponent<ProxyVert>();
                Vert mVert = walkboxes.mVerts[i];

                proxy_mVert.mFlags = mVert.mFlags.mFlags;
                wbox_vert_gameObject.transform.position = new UnityEngine.Vector3()
                {
                    x = mVert.mPos.x,
                    y = mVert.mPos.y,
                    z = mVert.mPos.z
                };

                wbox_vert_gameObject.transform.SetParent(wbox_parent_mVerts_gameObject.transform);

                proxy_wbox.mVerts.Add(proxy_mVert);
            }

            //-----------------------------------------
            //mNormals

            proxy_wbox.mNormals = new List<ProxyNormal>();

            GameObject wbox_parent_mNormals_gameObject = new GameObject("mNormals");
            wbox_parent_mNormals_gameObject.transform.SetParent(wbox_parent_gameObject.transform);
            proxy_wbox.mNormals_parent = wbox_parent_mNormals_gameObject;

            for (int i = 0; i < walkboxes.mNormals.Length; i++)
            {
                GameObject wbox_normal_gameObject = new GameObject(string.Format("mNormal {0}", i));
                ProxyNormal proxy_mNormal = wbox_normal_gameObject.AddComponent<ProxyNormal>();
                Vector3 mNormal = walkboxes.mNormals[i];

                UnityEngine.Vector3 unity_mNormal = new UnityEngine.Vector3()
                {
                    x = mNormal.x,
                    y = mNormal.y,
                    z = mNormal.z
                };

                proxy_mNormal.mNormal = unity_mNormal;

                wbox_normal_gameObject.transform.forward = unity_mNormal;

                wbox_normal_gameObject.transform.SetParent(wbox_parent_mNormals_gameObject.transform);

                proxy_wbox.mNormals.Add(proxy_mNormal);
            }

            //-----------------------------------------
            //mQuads

            proxy_wbox.mQuads = new List<ProxyQuad>();

            GameObject wbox_parent_mQuads_gameObject = new GameObject("mQuads");
            wbox_parent_mQuads_gameObject.transform.SetParent(wbox_parent_gameObject.transform);
            proxy_wbox.mQuads_parent = wbox_parent_mQuads_gameObject;

            for (int i = 0; i < walkboxes.mQuads.Length; i++)
            {
                GameObject wbox_quad_gameObject = new GameObject(string.Format("mQuad {0}", i));
                ProxyQuad proxy_mQuad = wbox_quad_gameObject.AddComponent<ProxyQuad>();
                Quad mQuad = walkboxes.mQuads[i];

                proxy_mQuad.mVerts = new List<ProxyVert>();

                for (int x = 0; x < mQuad.mVerts.Length; x++)
                {
                    int vertexIndex = mQuad.mVerts[x];

                    string vertexNameQuery = string.Format("mVert {0}", vertexIndex);

                    GameObject foundVertex = proxy_wbox.mVerts_parent.transform.Find(vertexNameQuery).gameObject;
                    ProxyVert foundVertexComponent = foundVertex.GetComponent<ProxyVert>();

                    proxy_mQuad.mVerts.Add(foundVertexComponent);
                }

                wbox_quad_gameObject.transform.SetParent(wbox_parent_mQuads_gameObject.transform);

                proxy_wbox.mQuads.Add(proxy_mQuad);
            }

            //-----------------------------------------
            //mTris

            proxy_wbox.mTris = new List<ProxyTri>();

            GameObject wbox_parent_mTris_gameObject = new GameObject("mTris");
            wbox_parent_mTris_gameObject.transform.SetParent(wbox_parent_gameObject.transform);
            proxy_wbox.mTris_parent = wbox_parent_mTris_gameObject;

            for (int i = 0; i < walkboxes.mTris.Length; i++)
            {
                GameObject wbox_tri_gameObject = new GameObject(string.Format("mTri {0}", i));
                ProxyTri proxy_mTri = wbox_tri_gameObject.AddComponent<ProxyTri>();
                Tri mTri = walkboxes.mTris[i];

                proxy_mTri.mFootstepMaterial = mTri.mFootstepMaterial;
                proxy_mTri.mFlags = mTri.mFlags.mFlags;

                //proxy_mTri.mNormal = mTri.mNormal;

                string normalNameQuery = string.Format("mNormal {0}", mTri.mNormal);
                GameObject foundNormal = proxy_wbox.mNormals_parent.transform.Find(normalNameQuery).gameObject;
                ProxyNormal foundNormalComponent = foundNormal.GetComponent<ProxyNormal>();

                proxy_mTri.mNormal = foundNormalComponent;

                proxy_mTri.mQuadBuddy = mTri.mQuadBuddy; //references a quad by index
                proxy_mTri.mMaxRadius = mTri.mMaxRadius;
                proxy_mTri.mVerts = new List<ProxyVert>();

                for (int x = 0; x < mTri.mVerts.Length; x++)
                {
                    int vertexIndex = mTri.mVerts[x];

                    string vertexNameQuery = string.Format("mVert {0}", vertexIndex);

                    GameObject foundVertex = proxy_wbox.mVerts_parent.transform.Find(vertexNameQuery).gameObject;
                    ProxyVert foundVertexComponent = foundVertex.GetComponent<ProxyVert>();

                    proxy_mTri.mVerts.Add(foundVertexComponent);
                }

                proxy_mTri.mEdgeInfo = new List<ProxyEdge>(); //SArray<WalkBoxes::Edge,3>

                for (int x = 0; x < mTri.mEdgeInfo.Length; x++)
                {
                    //-----------------------------------------
                    //mEdgeInfo

                    GameObject wbox_edge_gameObject = new GameObject(string.Format("mEdge {0}", x));
                    ProxyEdge proxy_mEdge = wbox_edge_gameObject.AddComponent<ProxyEdge>();
                    Edge mEdge = mTri.mEdgeInfo[x];

                    int mV1_vertexIndex = mEdge.mV1;
                    int mV2_vertexIndex = mEdge.mV2;

                    string mV1_vertexNameQuery = string.Format("mVert {0}", mV1_vertexIndex);
                    string mV2_vertexNameQuery = string.Format("mVert {0}", mV2_vertexIndex);

                    GameObject mV1_foundVertex = proxy_wbox.mVerts_parent.transform.Find(mV1_vertexNameQuery).gameObject;
                    GameObject mV2_foundVertex = proxy_wbox.mVerts_parent.transform.Find(mV2_vertexNameQuery).gameObject;

                    ProxyVert mV1_foundVertexComponent = mV1_foundVertex.GetComponent<ProxyVert>();
                    ProxyVert mV2_foundVertexComponent = mV2_foundVertex.GetComponent<ProxyVert>();

                    proxy_mEdge.mV1 = mV1_foundVertexComponent;
                    proxy_mEdge.mV2 = mV2_foundVertexComponent;

                    proxy_mEdge.mEdgeDest = mEdge.mEdgeDest;
                    proxy_mEdge.mEdgeDestEdge = mEdge.mEdgeDestEdge;
                    proxy_mEdge.mEdgeDir = mEdge.mEdgeDir;
                    proxy_mEdge.mMaxRadius = mEdge.mMaxRadius;

                    wbox_edge_gameObject.transform.SetParent(wbox_tri_gameObject.transform);

                    proxy_mTri.mEdgeInfo.Add(proxy_mEdge);
                }

                proxy_mTri.mVertOffsets = mTri.mVertOffsets; //SArray<int,3> 
                proxy_mTri.mVertScales = mTri.mVertScales; //SArray<float,3>

                wbox_tri_gameObject.transform.SetParent(wbox_parent_mTris_gameObject.transform);

                proxy_wbox.mTris.Add(proxy_mTri);
            }
        }

        private void GenerateUnityWBOX()
        {
            NavMeshTriangulation triangledNavMesh = NavMesh.CalculateTriangulation();

            GameObject wbox_parent_gameObject = new GameObject("NewNavMeshWalkbox.wbox");
            ProxyWalkboxes proxy_wbox = wbox_parent_gameObject.AddComponent<ProxyWalkboxes>();

            proxy_wbox.msv6 = new MSV6();
            proxy_wbox.msv5 = null;
            proxy_wbox.mtre = null;

            //-----------------------------------------
            //mVerts

            proxy_wbox.mVerts = new List<ProxyVert>();

            GameObject wbox_parent_mVerts_gameObject = new GameObject("mVerts");
            wbox_parent_mVerts_gameObject.transform.SetParent(wbox_parent_gameObject.transform);
            proxy_wbox.mVerts_parent = wbox_parent_mVerts_gameObject;

            for (int i = 0; i < triangledNavMesh.vertices.Length; i++)
            {
                GameObject wbox_vert_gameObject = new GameObject(string.Format("mVert {0}", i));
                ProxyVert proxy_mVert = wbox_vert_gameObject.AddComponent<ProxyVert>();
                UnityEngine.Vector3 newVertex = triangledNavMesh.vertices[i];

                proxy_mVert.mFlags = 0;
                wbox_vert_gameObject.transform.position = newVertex;

                wbox_vert_gameObject.transform.SetParent(wbox_parent_mVerts_gameObject.transform);

                proxy_wbox.mVerts.Add(proxy_mVert);
            }

            //-----------------------------------------
            //mNormals

            proxy_wbox.mNormals = new List<ProxyNormal>();

            GameObject wbox_parent_mNormals_gameObject = new GameObject("mNormals");
            wbox_parent_mNormals_gameObject.transform.SetParent(wbox_parent_gameObject.transform);
            proxy_wbox.mNormals_parent = wbox_parent_mNormals_gameObject;

            for (int i = 0; i < 1; i++)
            {
                GameObject wbox_normal_gameObject = new GameObject(string.Format("mNormal {0}", i));
                ProxyNormal proxy_mNormal = wbox_normal_gameObject.AddComponent<ProxyNormal>();

                UnityEngine.Vector3 unity_mNormal = new UnityEngine.Vector3()
                {
                    x = 0.0f,
                    y = 1.0f,
                    z = 0.0f
                };

                proxy_mNormal.mNormal = unity_mNormal;

                wbox_normal_gameObject.transform.forward = unity_mNormal;

                wbox_normal_gameObject.transform.SetParent(wbox_parent_mNormals_gameObject.transform);

                proxy_wbox.mNormals.Add(proxy_mNormal);
            }

            //-----------------------------------------
            //mQuads

            proxy_wbox.mQuads = new List<ProxyQuad>();

            GameObject wbox_parent_mQuads_gameObject = new GameObject("mQuads");
            wbox_parent_mQuads_gameObject.transform.SetParent(wbox_parent_gameObject.transform);
            proxy_wbox.mQuads_parent = wbox_parent_mQuads_gameObject;

            //-----------------------------------------
            //mTris

            proxy_wbox.mTris = new List<ProxyTri>();

            GameObject wbox_parent_mTris_gameObject = new GameObject("mTris");
            wbox_parent_mTris_gameObject.transform.SetParent(wbox_parent_gameObject.transform);
            proxy_wbox.mTris_parent = wbox_parent_mTris_gameObject;

            int edgeIndex = 0;

            for (int i = 0; i < triangledNavMesh.indices.Length; i += 3)
            {
                GameObject wbox_tri_gameObject = new GameObject(string.Format("mTri {0}", i));
                ProxyTri proxy_mTri = wbox_tri_gameObject.AddComponent<ProxyTri>();

                proxy_mTri.mFootstepMaterial = EnumMaterial.Default;
                proxy_mTri.mFlags = 0;

                string normalNameQuery = string.Format("mNormal {0}", 0);
                GameObject foundNormal = proxy_wbox.mNormals_parent.transform.Find(normalNameQuery).gameObject;
                ProxyNormal foundNormalComponent = foundNormal.GetComponent<ProxyNormal>();

                proxy_mTri.mNormal = foundNormalComponent;

                proxy_mTri.mQuadBuddy = -1; //references a quad by index
                proxy_mTri.mMaxRadius = 100000;

                //---------------------------------------------------------------------
                //assign verticies
                proxy_mTri.mVerts = new List<ProxyVert>();

                int vertexIndex1 = triangledNavMesh.indices[i + 0];
                int vertexIndex2 = triangledNavMesh.indices[i + 1];
                int vertexIndex3 = triangledNavMesh.indices[i + 2];

                string vertexNameQuery_1 = string.Format("mVert {0}", vertexIndex1);
                string vertexNameQuery_2 = string.Format("mVert {0}", vertexIndex2);
                string vertexNameQuery_3 = string.Format("mVert {0}", vertexIndex3);

                ProxyVert foundVertexComponent_1 = proxy_wbox.mVerts_parent.transform.Find(vertexNameQuery_1).gameObject.GetComponent<ProxyVert>();
                ProxyVert foundVertexComponent_2 = proxy_wbox.mVerts_parent.transform.Find(vertexNameQuery_2).gameObject.GetComponent<ProxyVert>();
                ProxyVert foundVertexComponent_3 = proxy_wbox.mVerts_parent.transform.Find(vertexNameQuery_3).gameObject.GetComponent<ProxyVert>();

                proxy_mTri.mVerts.Add(foundVertexComponent_1);
                proxy_mTri.mVerts.Add(foundVertexComponent_2);
                proxy_mTri.mVerts.Add(foundVertexComponent_3);

                //---------------------------------------------------------------------
                //creates edges
                proxy_mTri.mEdgeInfo = new List<ProxyEdge>();

                for (int x = 0; x < 3; x++)
                {
                    GameObject wbox_edge_gameObject = new GameObject(string.Format("mEdge {0}", x));
                    ProxyEdge proxy_mEdge = wbox_edge_gameObject.AddComponent<ProxyEdge>();

                    int mV1_vertexIndex = vertexIndex1;
                    int mV2_vertexIndex = vertexIndex2;

                    if(x == 1)
                    {
                        mV1_vertexIndex = vertexIndex2;
                        mV2_vertexIndex = vertexIndex3;
                    }
                    else if(x == 2)
                    {
                        mV1_vertexIndex = vertexIndex3;
                        mV2_vertexIndex = vertexIndex1;
                    }

                    string mV1_vertexNameQuery = string.Format("mVert {0}", mV1_vertexIndex);
                    string mV2_vertexNameQuery = string.Format("mVert {0}", mV2_vertexIndex);

                    GameObject mV1_foundVertex = proxy_wbox.mVerts_parent.transform.Find(mV1_vertexNameQuery).gameObject;
                    GameObject mV2_foundVertex = proxy_wbox.mVerts_parent.transform.Find(mV2_vertexNameQuery).gameObject;

                    ProxyVert mV1_foundVertexComponent = mV1_foundVertex.GetComponent<ProxyVert>();
                    ProxyVert mV2_foundVertexComponent = mV2_foundVertex.GetComponent<ProxyVert>();

                    proxy_mEdge.mV1 = mV1_foundVertexComponent;
                    proxy_mEdge.mV2 = mV2_foundVertexComponent;

                    proxy_mEdge.mEdgeDest = 0;
                    proxy_mEdge.mEdgeDestEdge = 0;
                    proxy_mEdge.mEdgeDir = edgeIndex;
                    proxy_mEdge.mMaxRadius = UnityEngine.Vector3.Distance(mV1_foundVertex.transform.position, mV2_foundVertex.transform.position);

                    wbox_edge_gameObject.transform.SetParent(wbox_tri_gameObject.transform);

                    proxy_mTri.mEdgeInfo.Add(proxy_mEdge);

                    edgeIndex++;
                }

                //---------------------------------------------------------------------

                proxy_mTri.mVertOffsets = new int[3] //SArray<int,3> 
                {
                    0,
                    0,
                    0
                };

                proxy_mTri.mVertScales = new float[3] //SArray<float,3>
                {
                    1.0f,
                    1.0f,
                    1.0f
                };


                wbox_tri_gameObject.transform.SetParent(wbox_parent_mTris_gameObject.transform);

                proxy_wbox.mTris.Add(proxy_mTri);
            }
        }
    }
}

#endif