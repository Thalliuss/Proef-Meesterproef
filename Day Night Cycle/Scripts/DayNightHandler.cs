using System;
using UnityEngine;

[Serializable]
public class DayNightHandler : MonoBehaviour
{
    public Light sun;
    public float speedMultiplier = 0.1f;
	public float startTime = 12.0f;

    private float _currentTime = 0.0f;
	private float _xValueOfSun = 90.0f;

	void Start () {
        _currentTime = startTime;
	}
	
	void Update () {
        _currentTime += Time.deltaTime * speedMultiplier;
		if (_currentTime >= 24.0f) {
            _currentTime %= 24.0f;
		}
		if (sun) {
			ControlLight();
		}
	}

	void ControlLight() {
        _xValueOfSun = -(90.0f+ _currentTime * 15.0f);
        sun.transform.eulerAngles = sun.transform.right* _xValueOfSun;
		if (_xValueOfSun >= 360.0f) {
            _xValueOfSun = 0.0f;
		}
	}
}
