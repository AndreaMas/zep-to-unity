using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSpawner : MonoBehaviour
{
    public GameObject camera1;
    public GameObject camera2;

    private Quaternion QuaternionFromMatrix(Matrix4x4 m)
    {
        Quaternion q = new Quaternion();
        float trace = m.m00 + m.m11 + m.m22;

        if (trace > 0f)
        {
            float s = Mathf.Sqrt(trace + 1f) * 2f; // S = 4 * q.w
            q.w = 0.25f * s;
            q.x = (m.m21 - m.m12) / s;
            q.y = (m.m02 - m.m20) / s;
            q.z = (m.m10 - m.m01) / s;
        }
        else if ((m.m00 > m.m11) && (m.m00 > m.m22))
        {
            float s = Mathf.Sqrt(1f + m.m00 - m.m11 - m.m22) * 2f; // S = 4 * q.x
            q.w = (m.m21 - m.m12) / s;
            q.x = 0.25f * s;
            q.y = (m.m01 + m.m10) / s;
            q.z = (m.m02 + m.m20) / s;
        }
        else if (m.m11 > m.m22)
        {
            float s = Mathf.Sqrt(1f + m.m11 - m.m00 - m.m22) * 2f; // S = 4 * q.y
            q.w = (m.m02 - m.m20) / s;
            q.x = (m.m01 + m.m10) / s;
            q.y = 0.25f * s;
            q.z = (m.m12 + m.m21) / s;
        }
        else
        {
            float s = Mathf.Sqrt(1f + m.m22 - m.m00 - m.m11) * 2f; // S = 4 * q.z
            q.w = (m.m10 - m.m01) / s;
            q.x = (m.m02 + m.m20) / s;
            q.y = (m.m12 + m.m21) / s;
            q.z = 0.25f * s;
        }
        return q;
    }

    // Start is called before the first frame update
    void Start()
    {

        // apply intrinsics

        float fovy = 41.591793f; // vertical field of view in deg
        float aspect = 1.333333f; // aspect ratio (width/height)

        Camera cam1 = camera1.GetComponent<Camera>();
        Camera cam2 = camera2.GetComponent<Camera>();

        cam1.aspect = aspect;
        cam1.fieldOfView = fovy;
        cam2.aspect = aspect;
        cam2.fieldOfView = fovy;

        // get the camera-to-world matrix

        Matrix4x4 wtcMat1 = new Matrix4x4
        { 
            m00 = -0.501707f,  m01 = 0.852907f,  m02 = 0.144362f,  m03 = -39.353165f,
            m10 = -0.634672f,  m11 = -0.476332f, m12 = 0.608522f,  m13 = 34.064927f,
            m20 = 0.587777f,   m21 = 0.213677f,  m22 = 0.780295f,  m23 = -267.349184f,
            m30 = 0.000000f,   m31 = 0.000000f,  m32 = 0.000000f,  m33 = 1.000000f
        };

        Matrix4x4 wtcMat2 = new Matrix4x4
        {
            m00 = -0.254407f,  m01 = 0.939124f,  m02 = 0.230917f, m03 = -46.644877f,
            m10 = -0.708302f,  m11 = -0.343513f, m12 = 0.616690f, m13 = 32.818378f,
            m20 = 0.658471f,   m21 = -0.006669f, m22 = 0.752576f, m23 = -281.476815f,
            m30 = 0.000000f,   m31 = 0.000000f,  m32 = 0.000000f, m33 = 1.000000f
        };

        wtcMat1 = wtcMat1.inverse;
        wtcMat2 = wtcMat2.inverse;

        // set position

        Vector3 position1 = new Vector3(
            wtcMat1.m03,
            wtcMat1.m13,
            wtcMat1.m23
        );
        Vector3 position2 = new Vector3(
            wtcMat2.m03,
            wtcMat2.m13,
            wtcMat2.m23
        );

        // set rotation

        Quaternion rotation1 = Quaternion.LookRotation(
            wtcMat1.GetColumn(2), // Forward (Z axis)
            wtcMat1.GetColumn(1)  // Up (Y axis)
        );
        Quaternion rotation2 = Quaternion.LookRotation(
            wtcMat2.GetColumn(2), // Forward (Z axis)
            wtcMat2.GetColumn(1)  // Up (Y axis)
        );

        //Quaternion rotation1 = QuaternionFromMatrix(wtcMat1);
        //Quaternion rotation2 = QuaternionFromMatrix(wtcMat1);

        // set position and rotation to cameras

        camera1.transform.SetPositionAndRotation(position1, rotation1);
        camera2.transform.SetPositionAndRotation(position2, rotation2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
