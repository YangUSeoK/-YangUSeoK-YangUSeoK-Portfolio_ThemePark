using UnityEngine;
using System.Collections;

public class OnCollisTriggers : MonoBehaviour {
	public Phone _Phone;
	public string NameTagTriggerPhone;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.tag == NameTagTriggerPhone) {
			if(other.gameObject.GetComponent<TriggersPhone>().SignalTrigger == true)
			{
				_Phone.SignalNum += other.gameObject.GetComponent<TriggersPhone>().NumSignal;
			}
		}
		if (other.gameObject.tag == NameTagTriggerPhone) {
			if(other.gameObject.GetComponent<TriggersPhone>().BatteryTrigger == true)
			{
				_Phone.BatteryNum += other.gameObject.GetComponent<TriggersPhone>().NumBattery;
				Destroy(other.gameObject);
			}
		}
		if (other.gameObject.tag == NameTagTriggerPhone) {
			if(other.gameObject.GetComponent<TriggersPhone>().SMSTrigger == true)
			{
				_Phone.TextSMS += other.gameObject.GetComponent<TriggersPhone>().TextSMS;
				_Phone.IconSMS.SetActive(true);
				if(!_Phone._AudioSource.isPlaying)
				{
					_Phone._AudioSource.clip = _Phone.ClipSMS;
					_Phone._AudioSource.Play();
				}
				Destroy(other.gameObject);
			}
		}
	}

	void OnTriggerExit (Collider other) {
		if (other.gameObject.tag == NameTagTriggerPhone) {
			if(other.gameObject.GetComponent<TriggersPhone>().SignalTrigger == true)
			{
				_Phone.SignalNum -= other.gameObject.GetComponent<TriggersPhone>().NumSignal;
			}
		}
	}
}
