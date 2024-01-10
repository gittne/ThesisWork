using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Fps : MonoBehaviour
{

	[SerializeField] int MaxFps = 60;

	void Awake()
	{
		QualitySettings.vSyncCount = 1;
		Application.targetFrameRate = MaxFps;
	}
}
