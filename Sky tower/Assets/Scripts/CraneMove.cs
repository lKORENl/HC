﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneMove : MonoBehaviour
{
    public GameObject blockCreator;
    [SerializeField] float maxDistanseTop = 1;

    [SerializeField] float upDownSpeed = 1f;
    float currentHeight = 0;
    float buffHeight = 0;
    float delay = 4f;
    float chanceForDelay = 70;
    float CurrentDelay = 4f;
    bool isMoveup = true;
    bool isPouse = false;



    float timAfterStar = 0f;
    /// <summary>
    /// Длина маятника, м
    /// </summary>
    internal float Length { get; private set; }
    /// <summary>
    /// Период колебаний маятника, сек
    /// </summary>
    internal float T { get; private set; }
    /// <summary>
    /// Фаза колебаний маятника (0 — 2П), рад
    /// </summary>
   // internal float Phase { get; private set; }

    private float A;//Амплитуда колебаний

    [SerializeField] float speed = 0.8f;
    [SerializeField] float angle = 10f;
    double n = 0;
    float X = 0;
    float phase = 0;

    bool showRes = false;

    public static CraneMove instance;

    float prevAngle = 0;
    bool toRight = false;
    // Start is called before the first frame update
    void Start()
    {

        if (instance == null)
            instance = this;

        blockCreator = GameObject.Find("BlockCreator");



        InitCrane(10);

        GameController.instance.craneController.Iinit();
    }

    // Update is called once per frame
    void Update()
    {

        tmr_Elapsed();
        MoveUpDownCrane();
    }




    /// <summary>
    /// Создание нового экземпляра маятника с заданной длиной
    /// </summary>
    /// <param name="length">Длина маятника в метрах</param>
    public void InitCrane(float length)
    {
        ChangeSpeed(0.2f);
        ChangeAngle(6);
        ChangeTopDownLength(0.3f, 0.5f);
        CameraController.hard = 1;

        this.Length = length;
        this.T = speed * (float)(Mathf.PI * Mathf.Sqrt(this.Length / 9.80665f));
        /// Максимальная амплитуда колебаний. Рассчитывается, исходя из
        /// максимального угла отклонения 8°
        A = this.Length * (float)Mathf.Sin(8 * Mathf.PI / 180);

    }

    void tmr_Elapsed()
    {

        this.T = speed * (float)(Mathf.PI * Mathf.Sqrt(this.Length / 9.80665f));
        A = this.Length * (float)Mathf.Sin(8 * Mathf.PI / 180);


        //фаза
        phase = timAfterStar / T;

        timAfterStar += Time.deltaTime;

        X = A * (float)Mathf.Sin(phase);

        //Пересчёт фазы от 0 до 2П
        n = phase / (2 * Mathf.PI);

        if (n % (int)n <= 0.009)
        {
            timAfterStar = 0;
        }
        prevAngle = transform.eulerAngles.z;

        transform.eulerAngles = new Vector3(0, 0, angle * X);

        if (prevAngle > transform.eulerAngles.z)
            toRight = false;
        else
            toRight = true;


    }


    private void MoveUpDownCrane()
    {
        if (!isPouse)
        {
            if (isMoveup)
            {
                if (currentHeight >= 0)
                {
                    float h = Time.deltaTime * upDownSpeed;
                    transform.position += new Vector3(0, h, 0);
                    currentHeight -= h;
                }
                else
                {
                    isMoveup = false;
                    float x = Random.Range(0, maxDistanseTop);

                    currentHeight = x + buffHeight;

                    buffHeight = x;

                    if (Random.Range(0, 100) <= chanceForDelay)
                        isPouse = true;
                }
            }
            else
            {
                if (currentHeight >= 0)
                {
                    float h = Time.deltaTime * upDownSpeed;
                    transform.position += new Vector3(0, -h, 0);
                    currentHeight -= h;
                }
                else
                {
                    isMoveup = true;
                    float x = Random.Range(0, maxDistanseTop);
                    currentHeight = x + buffHeight;

                    buffHeight = x;

                    if (Random.Range(0, 100) <= chanceForDelay)
                        isPouse = true;
                }

            }
        }
        else
        {
            if (CurrentDelay >= 0)
                CurrentDelay -= Time.deltaTime;
            else
            {
                CurrentDelay = delay;
                isPouse = false;
            }
        }
    }
    public void ChangeSpeed(float newSpeed)
    {
        speed = newSpeed;
        //this.T = speed * (float)(Mathf.PI * Mathf.Sqrt(this.Length / 9.80665f));
        //A = this.Length * (float)Mathf.Sin(8 * Mathf.PI / 180);


        //phase = timAfterStar / T;

        //X = A * (float)Mathf.Sin(phase);
        //int exit = 0;
        //Vector3 euler = new Vector3();

        //while (true)
        //{

        //    if (exit < 100)
        //        exit++;
        //    else
        //        break;

        //    timAfterStar += Time.deltaTime;
        //    phase = timAfterStar / T;

        //    X = A * (float)Mathf.Sin(phase);

        //    euler = new Vector3(0, 0, angle * X);

        //    Debug.Log("---------------- " + (360 - transform.eulerAngles.z - euler.z));

        //    if (360 - transform.eulerAngles.z - euler.z < 0.5f)
        //    {
        //        Debug.Log("1 " + (360 - transform.eulerAngles.z) + " " + euler.z);
        //        transform.eulerAngles = -euler;
        //        break;
        //    }

        //    if (transform.eulerAngles.z - euler.z < 0.5f)
        //    {
        //        Debug.Log("2 " + transform.eulerAngles.z + " " + euler.z);
        //        transform.eulerAngles = euler;
        //        break;
        //    }







        //}
        //Debug.Log(speed + " " + T + " " + X + " " + phase + " " + timAfterStar);
        //// showRes = true;
        //Debug.Log(exit + "-------------------------------");



    }

    public void ChangeAngle(float newAngle)
    {
        angle = newAngle;
    }
    public void ChangeTopDownLength(float distanse, float speed)
    {
        maxDistanseTop = distanse;
        upDownSpeed = speed;
    }

    public void ChangeCranHard(float newSpeed, float newAngle)
    {
        speed = newSpeed;
        angle = newAngle;


        this.T = speed * (float)(Mathf.PI * Mathf.Sqrt(this.Length / 9.80665f));
        A = this.Length * (float)Mathf.Sin(8 * Mathf.PI / 180);


        phase = timAfterStar / T;

        X = A * (float)Mathf.Sin(phase);
        
        Vector3 euler = new Vector3();

        float preAngleForCh = 0;

        int exit = 0;
        while (true)
        {

            if (exit < 200)
            {
                exit++;
            }
            else
            {
                Debug.Log("-----------------------------------");
                break;
            }


            
            phase = timAfterStar / T;

            timAfterStar += Time.deltaTime;

            X = A * (float)Mathf.Sin(phase);

            preAngleForCh = euler.z;

            euler = new Vector3(0, 0, angle * X);


            if (prevAngle > transform.eulerAngles.z && preAngleForCh > euler.z )
            {
                if (transform.eulerAngles.z > 200 )
                {

                   
                    float distance = Vector2.Distance(new Vector2(euler.z, 0), new Vector2(-(360 - transform.eulerAngles.z), 0));

                    if (distance < 0.3f )
                    {
                        Debug.Log(">>>>>>>>>>");
                        transform.eulerAngles = new Vector3(0,0,  euler.z);
                        break;
                    }
                }
                else
                {
                    if (Mathf.Abs(euler.z - transform.eulerAngles.z) < 0.3f)
                    {
                        Debug.Log(">break");
                        break;
                    }

                }

            }
            else if (prevAngle < transform.eulerAngles.z && preAngleForCh < euler.z)
            {
                if (transform.eulerAngles.z > 200 && euler.z < 0)
                {
                    float distance = Vector2.Distance(new Vector2(euler.z, 0), new Vector2(-(360 - transform.eulerAngles.z), 0));

                    if (distance < 0.3f)
                    {
                        Debug.Log("<<<<<<<<<<<<<<<<<");
                        transform.eulerAngles = new Vector3(0, 0, euler.z);
                        break;
                    }
                }
                else
                {
                    if (Mathf.Abs(euler.z - transform.eulerAngles.z) < 0.3f)
                    {
                        Debug.Log("<break");
                        break;
                    }
                }

            }

        }

    }


}
