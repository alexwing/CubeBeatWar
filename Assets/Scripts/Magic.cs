
/**
 * Script to create and shoot magic
 */

using System;
using System.Collections.Generic;
using UnityEngine;

public class Magic : MonoBehaviour
{
    public static Magic instance;

    public GameObject HeadAnchor;
    public GameObject LeftKameAnchor;
    public GameObject RightKameAnchor;

    public GameObject LeftHandAnchor;
    public GameObject RightHandAnchor;
    [SerializeField] private Transform _kames;

    [Header("Hand")]

    private Vector3 lastPosition;

    private Vector3 LastLeftHand;
    private Vector3 LastRightHand;

    [Header("Kame Hame Ha")]
    [Tooltip("Hand distance to init Kame Hame Ha.")]
    [Range(0f, 5f)]
    public float HandDistance = 0.1f;

    [Tooltip("Destroy distance from camera")]
    [Range(0f, 10000f)]
    public float _destroyDistance = 1000f;

    [Tooltip("Max size of Kame Hame Ha.")]
    [Range(0f, 8f)]
    public float _kameHameMaxSize = 2f;

    [Tooltip("Vertical position of Kame Hame Ha.")]
    [Range(-3f, 3f)]
    public float _kameHameHaPosition = 0.5f;
    [Header("Shoot")]
    [Tooltip("Hands intensity to launch kame.")]
    [Range(0f, 100f)]
    public float handIntensityToShoot = 1f;
    [Tooltip("Hands max intensity.")]
    [Range(0f, 100f)]
    public float handMaxIntensity = 10f;
    [Tooltip("Velocity min kame.")]
    [Range(0, 500000)]
    public int shootMinVelocity = 10000;

    [Tooltip("Velocity max kame.")]
    [Range(0, 500000)]
    public int shootMaxVelocity = 10000;

    [Header("Effect")]
    [SerializeField] private Transform[] _magicArray;
    private Transform currentKame;

    private float distance;
    private int index;

    private List<ParticleSystem> _magicParticleList;
    [Header("Sound")]
    public AudioSource AudioSourceKame;
    public AudioClip Create;
    public AudioClip Launch;
    private float _shotCount;
    public static float _hitCount;
    private static bool kameLeftHand = false;
    private static bool kameRightHand = false;

    public static void KameLeftOpened()
    {
        kameLeftHand = true;
    }
    public static void KameRightOpened()
    {
        kameRightHand = true;
    }
    public static void KameLeftClosed()
    {
        kameLeftHand = false;
    }
    public static void KameRightClosed()
    {
        kameRightHand = false;
    }

    void Awake()
    {
        instance = this;
        //OVRCameraRig 
        SceneConfig.MainCamera = GameObject.Find("OVRCameraRig").transform;
    }

    void Start()
    {
        _magicParticleList = new List<ParticleSystem>();
        _shotCount = 0;
        _hitCount = 0;
    }

    private void FixedUpdate()
    {
        //Debug.Log("isPaused: " + SceneConfig.gameIsPaused);
        if (SceneConfig.gameIsPaused)
        {
            return;
        }
        // Measure the distance between both palms
        bool validPosition = false;
        if (isValidController(OVRInput.Controller.LHand) && isValidController(OVRInput.Controller.RHand))
        {
            Vector3 leftHandValid = this.transform.TransformPoint(OVRInput.GetLocalControllerPosition((OVRInput.Controller.LHand)));
            Vector3 rightHandValid = this.transform.TransformPoint(OVRInput.GetLocalControllerPosition((OVRInput.Controller.RHand)));
            distance = Vector3.Distance(leftHandValid, rightHandValid);
            // limit kame hame size
            if (distance > _kameHameMaxSize)
            {
                distance = _kameHameMaxSize;
            }
            validPosition = true;
            //Debug.Log("distance: " + distance);
        }
        //Debug.Log("validPosition: " + validPosition);
        if (validPosition)
        {
            //without oculus sdk
            Vector3 middlePosition = Utils.CenterOfVectors(new Vector3[] { LeftKameAnchor.transform.position, RightKameAnchor.transform.position });
            middlePosition = new Vector3(middlePosition.x, middlePosition.y + _kameHameHaPosition, middlePosition.z);
            Vector3 middlePositionVelocity = Utils.CenterOfVectors(new Vector3[] { LeftHandAnchor.transform.localPosition, RightHandAnchor.transform.localPosition });



            //in oculus sdk this must works OVRInput.Controller.LHand and OVRInput.Controller.RHand
            //Vector3 middlePosition = Utils.CenterOfVectors(new Vector3[] { OVRInput.GetLocalControllerPosition((OVRInput.Controller.LHand)), OVRInput.GetLocalControllerPosition((OVRInput.Controller.RHand)) });
            //middlePosition = new Vector3(middlePosition.x, middlePosition.y + _kameHameHaPosition, middlePosition.z);
            //Vector3 middlePositionVelocity =  Utils.CenterOfVectors(new Vector3[] { OVRInput.GetLocalControllerPosition((OVRInput.Controller.LHand)), OVRInput.GetLocalControllerPosition((OVRInput.Controller.RHand)) });
            

            //float speed = Vector3.Distance( new Vector3(0,0,lastPosition.z), new Vector3(0,0,middlePosition.z)) / Time.deltaTime;
            //float speed = (middlePositionVelocity.z -lastPosition.z) / Time.deltaTime;

            float speed = Vector3.Distance(lastPosition, middlePositionVelocity) / Time.deltaTime;

            //log OVRInput.GetLocalControllerVelocity() 
            //Debug.Log("OVRInput.GetLocalControllerVelocity() left: " + OVRInput.GetLocalControllerVelocity((OVRInput.Controller.LHand)) + " right: " + OVRInput.GetLocalControllerVelocity((OVRInput.Controller.RHand)));

            //Debug.Log(" speed: " + speed);

            float distanceNow = Vector3.Distance(middlePositionVelocity, HeadAnchor.transform.localPosition);
            float distanceLast = Vector3.Distance(lastPosition, HeadAnchor.transform.localPosition);

            //only shoot if hands move to forward
           // if (distanceNow <= distanceLast)
           // {
           //     speed = 0;
          //  }

            Vector3 midway = Utils.CenterOfVectors(new Vector3[] { LeftKameAnchor.transform.forward, -RightKameAnchor.transform.forward, LastLeftHand, LastRightHand });
            
            //if inverse direction
           // if (Vector3.Dot(midway, middlePositionVelocity) < 0)
           // {
           //     speed = 0; 
           // }

            //Console.WriteLine("Speed: " + String.Format("{0:0.00}", speed));

            //update previous positions
            lastPosition = middlePositionVelocity;
            LastLeftHand = LeftKameAnchor.transform.forward;
            LastRightHand = -RightKameAnchor.transform.forward;

            //debug
            /* 
            Debug.Log("distance: "          + String.Format("{0:0.00}", distance)  
            + " speed rounded: "            + String.Format("{0:0.00}", speed) 
            + " distanceNow: "              + String.Format("{0:0.00}", distanceNow) 
            + " distanceLast: "             + String.Format("{0:0.00}", distanceLast)  
            + " midway: "                   + String.Format("{0:0.00}", midway) 
            + " middlePositionVelocity: "   + String.Format("{0:0.00}", middlePositionVelocity) 
            + " kame?: " + ((distance > HandDistance * 0.5f && distance < HandDistance) ? "yes" : "no") );
            */

            //if hands are near and not exist kame
            if (distance > HandDistance * 0.5f && distance < HandDistance && currentKame == null)
            {
                if (!kameLeftHand || !kameRightHand)
                {
                    DestroyKame();
                    return;
                }
                CreateEffect();
            }
            if (currentKame)
            {
                SizeMagic(middlePosition);
                ShootKame(midway, speed);
            }
        }
        else
        {
            DestroyKame();
        }
    }

    private bool isValidController(OVRInput.Controller controller)
    {
        if (OVRInput.GetControllerOrientationTracked(controller))
        {
            return true;
        }
        else
        {
            if (currentKame)
            {
                AudioSourceKame.Stop();
                Destroy(currentKame.gameObject);
                currentKame = null;
            }
            return false;
        }
    }

    private void CreateEffect()
    {
        Debug.Log("CreateEffect");
        // Generated after determining the effect at random
        index = UnityEngine.Random.Range(0, _magicArray.Length);
        currentKame = Instantiate(_magicArray[index], _kames.transform);
        currentKame.name = "kamehameha";
        currentKame.transform.parent = _kames;
        currentKame.GetComponent<KameHameHa>().Distance = _destroyDistance;

        //clean audio source
        AudioSourceKame.Stop();

        //play audio
        AudioSourceKame.clip = Create;
        AudioSourceKame.loop = true;
        AudioSourceKame.Play();

        _magicParticleList.Clear();

        // Included in a list consisting of several particles
        for (int i = 0; i < currentKame.childCount; i++)
            _magicParticleList.Add(currentKame.GetChild(i).GetComponent<ParticleSystem>());
    }

    private void SizeMagic(Vector3 middlePosition)
    {
        AudioSourceKame.pitch = distance;
        currentKame.position = middlePosition;
        // Pull out multiple particles from the list and scale them
        for (int i = 0; i < _magicParticleList.Count; i++)
            _magicParticleList[i].transform.localScale = new Vector3(distance, distance, distance) * 0.1f;
    }

    private void DestroyKame()
    {
        AudioSourceKame.Stop();
        if (currentKame)
        {
            Destroy(currentKame.gameObject);
            currentKame = null;
        }
    }

    private void ShootKame(Vector3 middlePosition, float speed)
    {
        if (speed > handIntensityToShoot && speed <= handMaxIntensity)
        {
            float launchSpeed = Mathf.InverseLerp(handIntensityToShoot, handMaxIntensity, speed);

            Console.WriteLine("launchSpeed: " + String.Format("{0:0.00}", launchSpeed));
            AudioSourceKame.Stop();

            Utils.PlaySound(Launch, currentKame, this.transform, 1000);
            float SpeedKame = Mathf.Lerp(0, shootMaxVelocity, launchSpeed);
            if (this.shootMinVelocity > SpeedKame)
            {
                SpeedKame = this.shootMinVelocity;
            }

            currentKame.GetComponent<KameHameHa>().Velocity = launchSpeed;
            currentKame.GetComponent<KameHameHa>().Size = distance * 100 / _kameHameMaxSize;
            currentKame.GetComponent<Rigidbody>().AddForce(middlePosition * SpeedKame);
            currentKame = null;
            _shotCount++;
        }
    }
}
