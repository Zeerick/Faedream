using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 
public class ParabolaRay : MonoBehaviour
{
    /*
    抛物线公式：y=a*x*x+b*x+c,由于z轴方向为正方向，所以：y=a*z*z+b*z+c,我们把抛出点设为原点（0，0），所以抛物线为y=a*z*z+b*z。
    */
    public float a = -0.0085f;//控制抛物线的开口和大小
    private float k;//k为抛物线上抛出点的切线 y=kx+d 的斜率，我们把抛出点设为原点，所以：y = kx，k = 2ax +b
    private float b;// 我们把抛出点设为坐标原点，所以由k = 2ax + b,b = k;
    public LineRenderer aimLazer; //抛物线的LineRenderer组件
    public int density = 50;//抛物线的精度
    public float space = 2;//每个节点间的间隔
                       // y = a*space*space+b*space
    
    public Vector3 posCompensation = new Vector3(0, 0, 0);    // A variable I added so that the initial of the curve can be set where the sorceries are casted.
    public Vector3 orientation = new Vector3(0, 0, 0);    // 3D direction of the parabola curve.
    
    private GameObject vertex;
    List<GameObject> vertices = new List<GameObject>();
    Vector3 prevPoint;
    void Start()
    {
        vertex= new GameObject();
        for (int i = 0; i < density; i++)
        {
            vertices.Add(Instantiate(vertex));
        }
        aimLazer.SetVertexCount(density+1);
        prevPoint = transform.position/* + posCompensation*/;    // Modified with posCompensation
    }

    private void OnEnable()
    {
        vertex= new GameObject();
        for (int i = 0; i < density; i++)
        {
            vertices.Add(Instantiate(vertex));
        }
        aimLazer.SetVertexCount(density+1);
        prevPoint = transform.position/* + posCompensation*/;    // Modified with posCompensation
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 f = orientation;
        f.y = 0;
        k = orientation.y/f.magnitude;//算出切线斜率（注意物体的旋转角度，当forward为垂直方向时，也就是f.magnitude的值为0的时候，应该为一条直线，这里没有考虑这个情况）
        b = k;
 
        bool cast = false;
        for (int i = 0; i < density; i++)
        {
            Vector3 p = GetPosition(i * space);
            vertices[i].transform.position = p;
            cast = Cast(p);
            if (cast)
            {
                break;
            }
        }
        if (!cast)
        {
 
        }
    }
    /// <summary>
    /// 根据z确定点坐标
    /// </summary>
    /// <param name="z"></param>
    /// <returns></returns>
    Vector3 GetPosition(float z)
    {
        float y = a * z * z + b * z;
        Vector3 f = orientation;
        f.y = 0;
        f = f.normalized;
 
        Vector3 pos = transform.position + f * z;//水平方向坐标
        pos.y = transform.position.y + y/* + posCompensation.y*/;//加上垂直方向坐标    // Modified with posCompensation
        
        return pos;
    }
     
    /// <summary>
    /// 进行抛物线检测
    /// </summary>
    /// <param name="currentPoint"></param>
    /// <returns></returns>
    bool Cast(Vector3 currentPoint)
    {
        Vector3 d = currentPoint - prevPoint;
        RaycastHit hit;
        if(Physics.Raycast(prevPoint, d.normalized, out hit, d.magnitude))
        {
            SetLine(hit.point);
            prevPoint = transform.position;    // Modified with posCompensation

            GameObject hitObject = hit.collider.gameObject;
            if (hitObject.CompareTag("Enemy") && !hitObject.GetComponent<EnemyHealth>().IsDead)
            {
                aimLazer.material.SetColor("_EmissionColor", Color.red);
            }
            else
            {
                aimLazer.material.SetColor("_EmissionColor", Color.cyan);
            }
            
            return true;
        }
        else
        {
            prevPoint = currentPoint;
            aimLazer.material.SetColor("_EmissionColor", Color.cyan);
            return false;
        }
    }
 
    /// <summary>
    /// 设置抛物线上每个点
    /// </summary>
    /// <param name="endPos"></param>
    void SetLine(Vector3 endPos)
    {
        Vector3 s = transform.position/* + posCompensation*/;    // Modified with posCompensation
        endPos.y = 0;
        s.y = 0;
 
        float j = Vector3.Distance(s, endPos) / density;
 
        for (int i = 0; i < density; i++)
        {
            aimLazer.SetPosition(i, GetPosition(i * j));
        }
        aimLazer.SetPosition(density, endPos);
        vertex.transform.position = endPos;
    }

    /// <summary>
    /// This method should be called once the parabola is not displayed, in order to clean up all the vertices to release memory.
    /// </summary>
    public void ClearVertices()
    {
        foreach (GameObject vertex in vertices)
        {
            UnityEngine.Object.Destroy(vertex);
        }
    }
}

