using UnityEngine;
using System.Collections;

public class destruction : MonoBehaviour {
	public GameObject[] Chunks;
	public GameObject[] HidingObjs; //список объектов, которые будут скрыты после давки.
	[Range(1,100)]
	public int Health = 100;
	public float ExplosionForce = 200; //сила, приложенная к каждому фрагменту разбитого объекта.
	public float ChunksRotation = 20; //сила вращения добавляется к каждому куску, когда он взрывается.
	public float strength = 5; //Как легко объект меняется.
	public bool BreakByClick = false;
	public bool DestroyAftertime = true; //если true, то куски будут уничтожены по истечении времени.
	public float time = 15; //время до того, как фрагменты будут удалены со сцены.
	public GameObject FX;
	public bool AutoDestroy = true; //если true, то объект будет автоматически разорван по истечении "AutoDestTime" с момента запуска игры.
	public float AutoDestTime = 2; //Время автоматического уничтожения (отсчитывается от начала игры).

	void Start () {

		if(AutoDestroy){
			Invoke("Crushing", AutoDestTime);
		}

		if(GetComponent<AudioSource>()){
			GetComponent<AudioSource>().pitch = Random.Range (0.7f, 1.1f);
		}
		if(HidingObjs.Length !=0){
			foreach(GameObject hidingObj in HidingObjs){
				hidingObj.SetActive(true);
			}
		}
	}

	void OnCollisionEnter(Collision other){
		if(other.gameObject.GetComponent<BulletsDamage>()){
		Health -= other.gameObject.GetComponent<BulletsDamage>().Damage;
			if(Health <= 0){
				Crushing();
				}
		}
		else if(other.relativeVelocity.magnitude > strength){
			Crushing();
		}

	}
	void OnMouseDown(){
		if(BreakByClick){
			Crushing();
			BreakByClick = false;
		}
		}

	void Crushing(){
		if(HidingObjs.Length !=0){
			foreach(GameObject hidingObj in HidingObjs){
				hidingObj.SetActive(false);
			}
		}
		if(FX){
			FX.SetActive(true);
		}
		if(GetComponent<AudioSource>()){
			GetComponent<AudioSource>().Play ();
		}
		
		if (GetComponent<Renderer>())
		{
			GetComponent<Renderer>().enabled = false;
		}

		GetComponent<Collider>().enabled = false;

		if (GetComponent<Rigidbody>())
		{
			GetComponent<Rigidbody>().isKinematic = true;
		}
		
		foreach(GameObject chunk in Chunks){
			chunk.SetActive(true);
			chunk.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * -ExplosionForce);
			chunk.GetComponent<Rigidbody>().AddRelativeTorque(Vector3.forward * -ChunksRotation*Random.Range(-5f, 5f));
			chunk.GetComponent<Rigidbody>().AddRelativeTorque(Vector3.right * -ChunksRotation*Random.Range(-5f, 5f));
		}
		if(DestroyAftertime){
			Invoke("DestructObject", time);
		}
	}

	void DestructObject(){
		Destroy(gameObject);
	}

}
