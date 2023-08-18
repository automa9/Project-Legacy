using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rukha93.Utilities
{
    public class RandomBliking : MonoBehaviour
    {
        public SkinnedMeshRenderer m_Mesh;
        public int[] blendshapes;
        public float closeTime = 0.1f;
        public float openTime = 0.1f;

        private IEnumerator Start()
        {
            var endOfFrame = new WaitForEndOfFrame();
            float t;
            float v;

            while (true)
            {
                //wait random time before blinking
                yield return new WaitForSeconds(Random.Range(1, 5));

                //close the eyes
                t = 0;
                while (t < closeTime)
                {
                    v = EaseOutCubic(t / closeTime) * 100;
                    t += Time.deltaTime;

                    yield return endOfFrame;

                    for (int i = 0; i < blendshapes.Length; i++)
                        m_Mesh.SetBlendShapeWeight(blendshapes[i], v);
                }

                //open
                t = openTime;
                while (t > 0)
                {
                    v = (t / openTime) * 100;
                    t -= Time.deltaTime;

                    yield return endOfFrame;

                    for (int i = 0; i < blendshapes.Length; i++)
                        m_Mesh.SetBlendShapeWeight(blendshapes[i], v);
                }

                for (int i = 0; i < blendshapes.Length; i++)
                    m_Mesh.SetBlendShapeWeight(blendshapes[i], 0);
            }
        }

        public float EaseOutCubic(float x)
        {
            return 1 - Mathf.Pow(1 - x, 3);
        }
    }
}
