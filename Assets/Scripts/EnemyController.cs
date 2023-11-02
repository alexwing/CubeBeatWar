
using System.Collections;
using UnityEngine;

namespace CubeBeatWar
{
    public class EnemyController : MonoBehaviour
    {
        //velocity
        public float velocity;
        public float moveToInitialHeightMin = 10f;
        public float moveToInitialHeightMax = 50f;
        private float moveToInitialHeight = 20f;

        public Vector3 endPosition;
        Vector3 target;
        [SerializeField] private Material attackMaterial;
        private Rigidbody rg;

        public Color color;
        public MeshRenderer meshRenderer;
    

        public void appearEnemy()
        {
            moveToInitialHeight = Random.Range(moveToInitialHeightMin * 1000, moveToInitialHeightMax * 1000) / 1000;
            Utils.ChangeColor(meshRenderer, color, "_BaseColor");
            Utils.ChangeColor(meshRenderer, color, "_EmissionColor");
            //random _Speed value for the enemy 0.0-1.0
            float speed = Random.Range(0, 1000) / 1000f;
            Utils.ChangeFloat(meshRenderer, speed, "_Speed");
            rg = GetComponent<Rigidbody>();
            target = endPosition;
            InvokeRepeating("goUp", 0, 0.025f);
        }

        //calculate target to main camera
        private Vector3 CalculateTarget()
        {

            return Camera.main.transform.position;
        }

        public void goUp()
        {

            //  Debug.Log("goToHead target position " + target.x + "," + target.y + "," + target.z);
            if (SceneConfig.gameIsPaused)
            {
                return;
            }

            //target = new Vector3(transform.position.x, moveToInitialHeight, transform.position.z);
            //verify distance to target
            if (Vector3.Distance(transform.position, target) > 0.1f)
            {
                    rg.AddForce(target - transform.position);
            }
            else
            {
                rg.AddForce(target - transform.position);
                //stop  goUp
                CancelInvoke("goUp");
            }
        }

        public void Hit(){
            LevelCreator.instance.enemyDeath(this);            
        }


        public void attack()
        {
            target = CalculateTarget();
            rg.AddForce(target - transform.position, ForceMode.VelocityChange);

            //atack material
            GetComponent<Renderer>().material = attackMaterial;
   


            /*if (transform.position.y < target.y)
            {
                CancelInvoke("moveToCamera");
            }*/
        }


        /*
                private void  Update()
                {

                    if (transform.position.y < moveToInitialHeight)
                    {
                         transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * velocity);

                    }else{
                        //send enemy to the camera
                        transform.position = Vector3.MoveTowards(transform.position, CalculateTarget(), Time.deltaTime * velocity);
                    }

                }
            */
    }
}



