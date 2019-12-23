using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Weather
{
    public string name;
    public Vector3 lightAngle;
    public Color lightColor;
    public Material skyBox;
    public GameObject extra;

    public void SetLight(Light directionalLight)
    {
        directionalLight.transform.eulerAngles = lightAngle;
        directionalLight.color = lightColor;
        RenderSettings.skybox = skyBox;
        if(extra != null)
            extra.SetActive(true);
    }

    public void DisableExtra()
    {
        if(extra != null)
            extra.SetActive(false);
    }
   
}

public class WeatherController : MonoBehaviour
{
    
    public Light directionalLight;
    public int currentWeather;
    public List<Weather> weatherList;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SetRandomWeather();
        }
    }
    
    public void SetRandomWeather()
    {
        var current = currentWeather;
        do
        {
            currentWeather = UnityEngine.Random.Range(0, weatherList.Count);
        } 
        while (currentWeather == current);

        weatherList[current].DisableExtra();
        weatherList[currentWeather].SetLight(directionalLight);
    }
}