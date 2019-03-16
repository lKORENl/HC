﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchZonesCreator : MonoBehaviour
{
    //ссылки на зоны в которых находятся фигуры
    Transform firstTouchZone;
    Transform secondTouchZone;
    Transform thirdTouchZone;

    List<GameObject> lo;


    public Slider slider;
    // Start is called before the first frame update

    //перемешивание списка временный метод выполняет роль дополнительного рандомайзера
    void MixList()
    {
        lo = new List<GameObject>();

        foreach (var item in Resources.LoadAll("Prefs/"))
        {
            lo.Add(item as GameObject);
        }
        int x = 0;

        for (int i = lo.Count - 1; i >= 1; i--)
        {
            x = Random.Range(0, lo.Count);
            var temp = lo[x];
            lo[x] = lo[i];
            lo[i] = temp;
        }
    }

    void Start()
    {
        MixList();
    }

    // Update is called once per frame
    void Update()
    {
        //если на поле нет ни одной фигуры
        if (transform.childCount == 0)
        {
            //генерируем новую волну
            GenerateNewWaveOfShape();
        }
    }

    //временный метод генерации новой волны фигур
    void GenerateNewWaveOfShape()
    {
        //смещение фигур относительно центра и ширины обьекта фигуры
        float x = 0;

        //получаем фигуру
        GameObject instance = GetNextShape();
        //назначаем родителем текущий обьект
        instance.transform.parent = transform;
        //выставляем скейл в базовое значение
        instance.transform.localScale = new Vector3(1, 1, 1);

        //получаем смещение
        x = instance.transform.position.x + instance.transform.GetComponent<RectTransform>().sizeDelta.x;
        //задаем новую позиции с учетом смещения
        instance.transform.localPosition = new Vector2(x, 0);
        


        //получаем вторую фигуру
        GameObject instance2 = GetNextShape();
        //назначаем ей родительским обьектом текущий
        instance2.transform.parent = transform;
        //выставляем скейл в базовое значение
        instance2.transform.localScale = new Vector3(1, 1, 1);


        //получаем третью фигуру
        GameObject instance3 = GetNextShape();
        //назначаем ей родительским обьектом текущий
        instance3.transform.parent = transform;
        //выставляем скейл в базовое значение
        instance3.transform.localScale = new Vector3(1, 1, 1);

        //получаем смещение
        x = instance3.transform.position.x + instance3.transform.GetComponent<RectTransform>().sizeDelta.x;
        //задаем новую позиции с учетом смещения
        instance3.transform.localPosition = new Vector2(-x, 0);
       
    }


    //создание трех фигур состоящих из одного блока используется после возрождения
    public void GenerateNewWaveOfShapeAfterRevive()
    {
        DestroyAllZones();

        float x = 0;
        GameObject instance = Instantiate(Resources.Load("Prefs/1", typeof(GameObject)), transform.position, Quaternion.identity) as GameObject;
        instance.transform.parent = transform;
        instance.transform.localScale = new Vector3(1, 1, 1);


        x = instance.transform.position.x + instance.transform.GetComponent<RectTransform>().sizeDelta.x;
        instance.transform.localPosition = new Vector2(instance.transform.localPosition.x + instance.transform.GetComponent<RectTransform>().sizeDelta.x, 0);




        GameObject instance2 = Instantiate(Resources.Load("Prefs/1", typeof(GameObject)), transform.position, Quaternion.identity) as GameObject;
        instance2.transform.parent = transform;
        instance2.transform.localScale = new Vector3(1, 1, 1);



        GameObject instance3 = Instantiate(Resources.Load("Prefs/1", typeof(GameObject)), transform.position, Quaternion.identity) as GameObject;
        instance3.transform.parent = transform;
        instance3.transform.localScale = new Vector3(1, 1, 1);

        x = instance3.transform.position.x + instance3.transform.GetComponent<RectTransform>().sizeDelta.x;
        instance3.transform.localPosition = new Vector2(-x, 0);


    }


    //создание волны после возрождени. Создание по Id фигуры
    public void GenerateNewWaveOfShapeAfterRevive(int[] shapesId)
    {
        DestroyAllZones();
      //  Debug.Log("Created");


        float x = 0;
        GameObject instance = ShapesManager.GetShapeById(shapesId[0],transform);
        instance.transform.parent = transform;
        instance.transform.localScale = new Vector3(1, 1, 1);


        x = instance.transform.position.x + instance.transform.GetComponent<RectTransform>().sizeDelta.x;
        instance.transform.localPosition = new Vector2(instance.transform.localPosition.x + instance.transform.GetComponent<RectTransform>().sizeDelta.x, 0);




        GameObject instance2 = ShapesManager.GetShapeById(shapesId[1], transform);
        instance2.transform.parent = transform;
        instance2.transform.localScale = new Vector3(1, 1, 1);



        GameObject instance3 = ShapesManager.GetShapeById(shapesId[2], transform);
        instance3.transform.parent = transform;
        instance3.transform.localScale = new Vector3(1, 1, 1);

        x = instance3.transform.position.x + instance3.transform.GetComponent<RectTransform>().sizeDelta.x;
        instance3.transform.localPosition = new Vector2(-x, 0);


    }

    //временный метод рандомного получения новой фигуры
    GameObject GetNextShape()
    {
        int x =  Random.Range(0, lo.Count);
        if (transform.childCount == 2)
            MixList();
        //int x = Random.Range(0, lo.Count);

        // return Instantiate(Resources.Load("Shapes/"+x, typeof(GameObject)), transform.position, Quaternion.identity) as GameObject;
        return Instantiate(lo[x], transform.position, Quaternion.identity) as GameObject;
    }


    //удаление содержимого обьекта в котором находятся фигуры
    public static void DestroyAllZones(Transform touchZonesParent)
    {

        for (int i = touchZonesParent.childCount-1; i >= 0; i--)
        {
            GameObject.Destroy(touchZonesParent.GetChild(i).gameObject);
        }
    }

    //удаление зон с фигурами
    public  void DestroyAllZones()
    {

        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            GameObject.Destroy(transform.GetChild(i).gameObject);
        }
    }

    
}
