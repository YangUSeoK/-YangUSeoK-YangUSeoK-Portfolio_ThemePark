using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Phone : MonoBehaviour {
	public Animation AnimationPhone;
	public AnimationClip ClipUpPhone, ClipDownPhone;
	private int ModePhone = 0;
	public GameObject CanvasPhone;
	public GameObject Signal, Battery, OperatorText, NumberText, IconConnecting, IconSMS;
	private int Mode;
	public string TextSMS;
	public Text TextSMS_UI;
	public Animation[] AnimationsComponets;
	public AnimationClip ClipAnimClick;
	private Animation animEmpty;
	public string[] Numbers;
	public AudioClip[] NumberSounds;
	public AudioClip ClipNotNumberMas, ClipClickBut, ClipSMS;
	public AudioSource _AudioSource;
	
	public int SignalNum = 20, BatteryNum = 40;
	public Image SignalImg1, SignalImg2, SignalImg3, SignalImg4;
	public Image BatteryImg1, BatteryImg2, BatteryImg3, BatteryImg4;
	public float timeDisablePhone;
	public GameObject PhoneObject;
	private float tm;
	// Use this for initialization
	void Start () {
		animEmpty = AnimationsComponets [0];
	}
	
	// Update is called once per frame
	void Update () {
		if (ModePhone == 0) {
			if (!AnimationPhone.isPlaying) {
				if(tm < timeDisablePhone)
				{
					tm += Time.deltaTime;
					if(tm >= timeDisablePhone)
					{
						PhoneObject.SetActive(false);
					}
				}
				
			}
			if (Input.GetKeyDown (KeyCode.T)) {
				if (!AnimationPhone.isPlaying) {
					PhoneObject.SetActive(true);
					AnimationPhone.clip = ClipUpPhone;
					AnimationPhone.Play();
					ModePhone = 1;
					tm = 0;
				}
			}
		}
		
		if (ModePhone == 1) {
			if (Input.GetKeyDown (KeyCode.T)) {
				if (!AnimationPhone.isPlaying) {
					AnimationPhone.clip = ClipDownPhone;
					AnimationPhone.Play();
					ModePhone = 0;
				}
			}
		}
		
		if (BatteryNum > 100) {
			BatteryNum = 100;
		}
		if (BatteryNum > 1) {
			if(CanvasPhone.activeSelf == false)
			{
				CanvasPhone.SetActive(true);
			}
		}
		if (BatteryNum < 1) {
			if(CanvasPhone.activeSelf == true)
			{
				CanvasPhone.SetActive(false);
			}
		}
		if (BatteryNum > 1) {
			BatteryImg1.enabled = true;
		}else {
			BatteryImg1.enabled = false;
		}
		if (BatteryNum > 25) {
			BatteryImg2.enabled = true;
		}else {
			BatteryImg2.enabled = false;
		}
		if (BatteryNum > 50) {
			BatteryImg3.enabled = true;
		}else {
			BatteryImg3.enabled = false;
		}
		if (BatteryNum > 75) {
			BatteryImg4.enabled = true;
		}else {
			BatteryImg4.enabled = false;
		}
		//----------------------------
		if (SignalNum > 1) {
			SignalImg1.enabled = true;
		}else {
			SignalImg1.enabled = false;
		}
		if (SignalNum > 25) {
			SignalImg2.enabled = true;
		}else {
			SignalImg2.enabled = false;
		}
		if (SignalNum > 50) {
			SignalImg3.enabled = true;
		}else {
			SignalImg3.enabled = false;
		}
		if (SignalNum > 75) {
			SignalImg4.enabled = true;
		}else {
			SignalImg4.enabled = false;
		}
		//----------------------------
		if (ModePhone == 1) {
			if (Input.GetKeyDown (KeyCode.Alpha0)) {
				OnPlayAnimBut (0);
			}
			if (Input.GetKeyDown (KeyCode.Alpha1)) {
				OnPlayAnimBut (1);
				
			}
			if (Input.GetKeyDown (KeyCode.Alpha2)) {
				OnPlayAnimBut (2);
				
			}
			if (Input.GetKeyDown (KeyCode.Alpha3)) {
				OnPlayAnimBut (3);
				
			}
			if (Input.GetKeyDown (KeyCode.Alpha4)) {
				OnPlayAnimBut (4);
				
			}
			if (Input.GetKeyDown (KeyCode.Alpha5)) {
				OnPlayAnimBut (5);
				
			}
			if (Input.GetKeyDown (KeyCode.Alpha6)) {
				OnPlayAnimBut (6);
				
			}
			if (Input.GetKeyDown (KeyCode.Alpha7)) {
				OnPlayAnimBut (7);
				
			}
			if (Input.GetKeyDown (KeyCode.Alpha8)) {
				OnPlayAnimBut (8);
				
			}
			if (Input.GetKeyDown (KeyCode.Alpha9)) {
				OnPlayAnimBut (9);
				
			}
			if (Input.GetKeyDown (KeyCode.Backspace)) {
				OnDeletedNum (10);
				
			}
			if (Mode == 1) {
				if (Input.GetKeyDown (KeyCode.Return)) {
					TextSMS_UI.text = "";
					Signal.SetActive (true);
					Battery.SetActive (true);
					OperatorText.SetActive (true);
					Mode = 0;
				}
			}
			if (Input.GetKeyDown (KeyCode.Return)) {
				if (Mode == 0) {
					if (NumberText.GetComponent<Text> ().text.Length > 0) {
						int n = 0;
						IconConnecting.SetActive (true);
						for (int i = 0; i < Numbers.Length; i++) {
							if (NumberText.GetComponent<Text> ().text == Numbers [i]) {
								n = 1;
								if (_AudioSource != null) {
									_AudioSource.clip = NumberSounds [i];
									_AudioSource.Play ();
								}
								i = Numbers.Length;
								Mode = -1;
							}
						}
						if (n == 0) {
							if (_AudioSource != null) {
								_AudioSource.clip = ClipNotNumberMas;
								_AudioSource.Play ();
								Mode = -1;
							}
						}
					}
				}
				
				if (Mode == 0) {
					if (IconSMS.activeSelf == true) {
						Signal.SetActive (false);
						Battery.SetActive (false);
						OperatorText.SetActive (false);
						TextSMS_UI.text = TextSMS;
						TextSMS = "";
						IconSMS.SetActive (false);
						Mode = 1;
					}
				}
			}
			
			if (Mode == -1) {
				if (!_AudioSource.isPlaying) {
					IconConnecting.SetActive (false);
					NumberText.GetComponent<Text> ().text = "";
					Signal.SetActive (true);
					Battery.SetActive (true);
					OperatorText.SetActive (true);
					if(TextSMS != "")
					{
						IconSMS.SetActive (true);
					}
					Mode = 0;
				}
			}
		}
		
	}
	
	void OnPlayAnimBut (int nm) {
		if (Mode == 0) {
			if(animEmpty != null)
			{
				if(NumberText.GetComponent<Text>().text.Length < 20)
				{
					if(!animEmpty.isPlaying)
					{
						if(_AudioSource != null)
						{
							_AudioSource.clip = ClipClickBut;
							_AudioSource.Play();
						}
						NumberText.GetComponent<Text>().text += nm;
						if(Signal.activeSelf == true)
						{
							Signal.SetActive(false);
							Battery.SetActive(false);
							OperatorText.SetActive(false);
							IconSMS.SetActive (false);
						}
						
						AnimationsComponets[nm].clip = ClipAnimClick;
						AnimationsComponets[nm].Play();
						animEmpty = AnimationsComponets[nm];
					}
				}
			}
		}
	}
	
	void OnDeletedNum (int nm) {
		if (Mode == 0) {
			if(animEmpty != null)
			{
				if(NumberText.GetComponent<Text>().text.Length > 0)
				{
					if(!animEmpty.isPlaying)
					{
						if(_AudioSource != null)
						{
							_AudioSource.clip = ClipClickBut;
							_AudioSource.Play();
						}
						NumberText.GetComponent<Text>().text = NumberText.GetComponent<Text>().text.Remove(NumberText.GetComponent<Text>().text.Length-1,1);
						
						AnimationsComponets[nm].clip = ClipAnimClick;
						AnimationsComponets[nm].Play();
						animEmpty = AnimationsComponets[nm];
						
						if(NumberText.GetComponent<Text>().text.Length == 0)
						{
							IconConnecting.SetActive(false);
							NumberText.GetComponent<Text>().text = "";
							Signal.SetActive(true);
							Battery.SetActive(true);
							OperatorText.SetActive(true);
						}
					}
				}
			}
		}
	}
}
