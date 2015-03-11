using UnityEngine;
using System.Collections;

public class FractalControls : MonoBehaviour
{
	/*
    VoxelSystemGreedy vc;
    float step = 0.2f;
    float n = 1f;
    float shiftStep = 0.01f;
    float shift = 0.0f;
    float scale = 1.0f;
    float colorManip = 1.75f;
    Vector3 zoomLocation;
    Ray mouseRay;
    RaycastHit hit;
    bool init = false;
    bool inverted = false;
    // Use this for initialization
    void Start()
    {
		vc = gameObject.GetComponent<VoxelSystemGreedy>();
    }

    // Update is called once per frame
    void Update()
    {
        if (vc.Initialized && !init)
        {
            Fractalize();
            init = true;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            n += step;
            zoomLocation = Vector3.zero;
            shift = 0;
            scale = 1;
            Debug.Log("n = " + n);
            Fractalize();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            n -= step;
            zoomLocation = Vector3.zero;
            shift = 0;
            scale = 1;
            Debug.Log("n = " + n);
            if (n < step) n = step;
            Fractalize();
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            shift += shiftStep;
            Fractalize();
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            shift -= shiftStep;
            Fractalize();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            inverted = !inverted;
            Fractalize();
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            scale /= 1.2f;
            Fractalize();
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            scale *= 1.2f;
            Fractalize();
        }
        if (Input.GetMouseButtonDown(1) && Input.GetKey(KeyCode.Z))
        {
            mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(mouseRay, out hit, 200))
            {
                VoxelPos pos = hit.transform.gameObject.GetComponent<VoxChunkManager>().RayCastHitToVoxelSpace(hit);
                zoomLocation.x = (pos.x + vc.offset.x / 2.0f);
                zoomLocation.y = (pos.y + vc.offset.y / 2.0f);
                zoomLocation.z = (pos.z + vc.offset.z / 2.0f);
                Fractalize();
            }
        }

    }

    void Fractalize()
    {
        for (int x = 1; x < vc.XSize - 1; x++)
        {
            for (int y = 1; y < vc.YSize - 1; y++)
            {
                for (int z = 1; z < vc.ZSize - 1; z++)
                {
                    VoxelFactory.GenerateVoxel(FractalBULLSHIT(x, y, z), ref vc.blocks[x, y, z], vc.offset, vc.VoxelSpacing);
                }
            }
        }
        vc.UpdateMesh();
    }

    int FractalBULLSHIT(int _x, int _y, int _z)
    {
        float x = (_x + zoomLocation.x) * scale;
        float y = (_y + zoomLocation.y) * scale;
        float z = (_z + zoomLocation.z) * scale;


        float r;
        float theta;

        float phi;
        Vector3 sum;
        Vector3 acc = new Vector3();
        Vector3 lastSum;
        sum = new Vector3((x + vc.offset.x) / (vc.XSize / 2.0f)
                          ,(y + vc.offset.y) / (vc.YSize / 2.0f)
                          ,(z + vc.offset.z) / (vc.ZSize / 2.0f));

        for (float i = 0; i < n * step; i += step)
        {
            r = sum.magnitude;
            theta = Mathf.Atan2(Mathf.Sqrt(sum.x * sum.x + sum.y * sum.y), sum.z);
            phi = Mathf.Atan2(sum.y, sum.x);
            lastSum = sum;
            sum += new Vector3(r * r * Mathf.Sin(theta * n * shift) * Mathf.Cos(phi * n)
                               , r * r * Mathf.Sin(theta * n * shift) * Mathf.Sin(phi * n )
                               , r * r * Mathf.Cos(theta * n *shift));
            acc += sum - lastSum;
        }
        //print(acc.ToString());
        if (!inverted)
            if (sum.magnitude < 2)
            {
                int count = 0;
                int loopCuttoff = vc.factory.VoxelMats.Count;
                int loopcount = 1;
                do
                {
                    if (acc.magnitude >= float.MaxValue)
                    {
                        count = vc.factory.VoxelMats.Count - 1;
                        break;
                    }
                    else
                    {
                        if (acc.magnitude < Mathf.Pow(Mathf.Sqrt(n*count), count / n / colorManip))
                            break;
                        else
                        {
                            count++;
                            if (count >= vc.factory.VoxelMats.Count) count = 1;
                        }

                        if (loopcount >= loopCuttoff)
                        {
                            loopcount = 1;
                            break;
                        }
                        loopcount++;
                    }
                } while (true);
                return count;
            }
            else
            {
                return 0;
            }
        if (inverted)
            if (sum.magnitude > 2)
            {
                int count = 1;
                int loopCuttoff = vc.factory.VoxelMats.Count;
                int loopcount = 1;
                do
                {
                    if (acc.magnitude >= float.MaxValue)
                    {
                        count = vc.factory.VoxelMats.Count - 1;
                        break;
                    }
                    else
                    {
                        if (acc.magnitude < Mathf.Pow(Mathf.Sqrt(n * count), count / n * 0.75f))
                            break;
                        else
                        {
                            count++;
                            if (count >= vc.factory.VoxelMats.Count) count = 1;
                        }

                        if (loopcount >= loopCuttoff)
                        {
                            loopcount = 0;
                            break;

                        }
                        loopcount++;
                    }
                } while (true);
                return count;
            }
            else
            {
                return 0;
            }
        return 0;

    }*/
}
