#if UNITY_EDITOR

using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace UnityTelltaleTools
{
    public class UnityWBOX : EditorWindow
    {
        private WalkBoxes_Master walkboxes_master;

        //add a menu item at the top of the unity editor toolbar
        [MenuItem("Telltale/Import .wbox")]
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
            if (GUILayout.Button("Import Walkbox"))
            {
                CreateUnityWBOX();
            }
        }

        private void CreateUnityWBOX()
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
                proxy_mTri.mNormal = mTri.mNormal;
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
    }
}

#endif