using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

namespace SlimUI.ModernMenu{
	public class OptionsMenu : MonoBehaviour {

		// sliders
		public bool isMainMenu;
		public GameObject musicSlider;
		public GameObject sensitivityXSlider;
		public GameObject sensitivityYSlider;

		private float sliderValue = 0.0f;
		private float sliderValueXSensitivity = 0.0f;
		private float sliderValueYSensitivity = 0.0f;

		Camera_Control[] cameraControls;
		AudioSource musicSource;

		public void  Start (){
			Time.timeScale = 1f;
            if (!isMainMenu)
            {
				cameraControls = GameObject.FindGameObjectWithTag("Player").GetComponentsInChildren<Camera_Control>();
			}
			musicSource = Camera.main.GetComponent<AudioSource>();

			if (!PlayerPrefs.HasKey("MusicVolume"))
            {
				PlayerPrefs.SetFloat("MusicVolume", 0.1f);
			}

			if (!PlayerPrefs.HasKey("XSensitivity"))
			{
				PlayerPrefs.SetFloat("XSensitivity", 10f);
			}

			if (!PlayerPrefs.HasKey("YSensitivity"))
			{
				PlayerPrefs.SetFloat("YSensitivity", 10f);
			}

			// check slider values
			musicSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("MusicVolume");
			sensitivityXSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("XSensitivity");
			sensitivityYSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("YSensitivity");

			musicSlider.GetComponent<Slider>().maxValue = .17f;
			sensitivityXSlider.GetComponent<Slider>().maxValue = 20f;
			sensitivityYSlider.GetComponent<Slider>().maxValue = 20f; 
			
			sliderValue = musicSlider.GetComponent<Slider>().value;
			sliderValueXSensitivity = sensitivityXSlider.GetComponent<Slider>().value;
			sliderValueYSensitivity = sensitivityYSlider.GetComponent<Slider>().value;

			musicSource.volume = sliderValue;
			if (!isMainMenu)
			{
				foreach (Camera_Control cam in cameraControls)
				{
					cam.sensHorizontal = sliderValueXSensitivity;
					cam.sensVertical = sliderValueYSensitivity;
				}
			}
		}

		public void  Update (){
			if (PauseBehavior.isPaused || isMainMenu)
            {
				sliderValue = musicSlider.GetComponent<Slider>().value;
				sliderValueXSensitivity = sensitivityXSlider.GetComponent<Slider>().value;
				sliderValueYSensitivity = sensitivityYSlider.GetComponent<Slider>().value;

				PlayerPrefs.SetFloat("MusicVolume", sliderValue);
				PlayerPrefs.SetFloat("XSensitivity", sliderValueXSensitivity);
				PlayerPrefs.SetFloat("YSensitivity", sliderValueYSensitivity);

				musicSource.volume = sliderValue;
				if (!isMainMenu)
                {
					foreach (Camera_Control cam in cameraControls)
					{
						cam.sensHorizontal = sliderValueXSensitivity;
						cam.sensVertical = sliderValueYSensitivity;
					}
				}
			}
		}
	}
}