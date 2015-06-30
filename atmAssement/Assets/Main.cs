using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;

using SimpleJSON;
/*
 *What works: Everything but Collision Detection,Button Images and TextMesh Labels
 * 			   JSON is Parsed and updates all the values
 * 			   All the buttons create the proper objects based on values sent
 * 			   All Game Objects Have physics (But can't trigger hit detections)
 * 			   All Color is created properly despite un-uniform string values in the hex
 * 			   Bounding Box is Invisible and keeps objects in proper place
 * 			   Gravity is properly set
 *			   Objects have a limited number 
 *			   Title is set from JSON
 *			   Reset Button Clears Objects (But doesn't tally collisions, causes a glitch when abused
 *What Doesn't:
 *				The Collisions have not been implemented yet, I beleive its because the rigidbody
 *				causes problems with the bounding box (It breaks apart when box collider isTrigger 
 *				is true) I imagine I could have solved this with a better bounding box, 
 *			    All that was needed was to get the onCollision Event to work and tally based on name
 *				But as of now it is not included. 
 *				The TextMeshs gave me the most trouble, I try  mulitple ways to get it to work
 *				but the issue was not resolving. I added the TextMesh and MeshRender
 *			    components to the objects, but they refused to appear. Other methods were also 
 *				fruitless, It became to time consuming so I moved on to the rest of the project.
 *				The button images were not setup due to mistake. the url lead me to a error 
 *				the last time I tried and I could not troubleshoot it before. Most likely just
 *			    needed to convert the images from the URL string to the proper image format.
 *				But it was abandoned to meet the deadline. 
 *
 *I feel like I both accomplished a lot and too little with this project, It was a pleasure to 
 *work on it and I am reluctant that I could not fully realize it before the time was up. I would
 *Say it took me about 8-12 hours to finish what I have done, My biggest mistake was working more
 *Intermittently then I should have. But everything is a learning experience and this was a grand 
 *one, It shows me that I still need to nuture my skills and my work habits to truly excel. 
 *I hope you had as much pleasure meeting me as I did meeting you. 
 *
 *Patrick Trenery 				
 *				
 * */

public class Main: MonoBehaviour {
	
	// Use this for initialization
	public Button btn, btn1, btn2, btn3, btn4, btn5;
	
	public Button reset;
	public Text title;
	public string btnName;
	int maxvalue = 3;
	int objNumber = 0;
	IEnumerator SendRequest() {
		WWW atmJSON = new WWW("http://atm.s3.amazonaws.com/Unity/UnityTest.json");//Json Script
		yield
			return atmJSON;
		var N = JSON.Parse(atmJSON.text);//Parse using SimpleJSON to create N
		
		string titleName = N["title"];//Set Title 
		
		
		N["buttons"][4]["color"] = "FFAA00";//Missing from JSON File for the orange sphere
		maxvalue = int.Parse(N["maxObjects"]);// Grab Max Objects 
		title.text = titleName;// Set title to text 
		
		//Sloppy Button declariations, set to JSON script 
		btn.GetComponentInChildren < Text > ().text = N["buttons"][0]["title"];
		btn1.GetComponentInChildren < Text > ().text = N["buttons"][1]["title"];
		btn2.GetComponentInChildren < Text > ().text = N["buttons"][2]["title"];
		btn3.GetComponentInChildren < Text > ().text = N["buttons"][3]["title"];
		btn4.GetComponentInChildren < Text > ().text = N["buttons"][4]["title"];
		btn5.GetComponentInChildren < Text > ().text = N["buttons"][5]["title"];
	   //I forgot about the button Images at the last minute. Last I checked I couldn't acccess them
	   //So they got left out by mistake, the code is similar to the text, but the image needs to be converted
	   //before it can be set to button
		
		//Sloppy Event Handlers for Buttons 
		btn.onClick.AddListener(() => {
			//Generate Object to pass values to function that will build the actual gameObjects 
			generateObject(N["buttons"][0]["type"], N["buttons"][0]["color"], N["buttons"][0]["obeyGravity"]);
			
		});
		
		btn1.onClick.AddListener(() => {// => nifty operand for creating handler
			
			generateObject(N["buttons"][1]["type"], N["buttons"][1]["color"], N["buttons"][0]["obeyGravity"]);
			
		});
		btn2.onClick.AddListener(() => {
			
			
			generateObject(N["buttons"][2]["type"], N["buttons"][2]["color"], N["buttons"][2]["obeyGravity"]);
			
		});
		
		btn3.onClick.AddListener(() => {
			
			
			generateObject(N["buttons"][3]["type"], N["buttons"][3]["color"], N["buttons"][3]["obeyGravity"]);
			
		});
		
		btn4.onClick.AddListener(() => {
			
			
			generateObject(N["buttons"][4]["type"], N["buttons"][4]["color"], N["buttons"][4]["obeyGravity"]);
			
		});
		btn5.onClick.AddListener(() => {
			
			
			generateObject(N["buttons"][5]["type"], N["buttons"][5]["color"], N["buttons"][5]["obeyGravity"]);
			
		});
		reset.onClick.AddListener(() => {
			
			int removingObj = objNumber;
			for (int i = 0; i < objNumber + 1; i++) {
				Destroy(GameObject.Find("Sphere" + i));
				Destroy(GameObject.Find("Cube" + i));
				removingObj--;
				
			}
			//StartCoroutine(SendRequest());
			objNumber = removingObj;
		//StartCorutine is causing a error where created objects double each clear 
		
			


			
			
			
			
		});
		
		
	}
	void Start() {
		
		//Grab JSON FILE
		StartCoroutine(SendRequest());
		
		
		
	}
	
	void generateObject(string primitiveType, string colorValue, string obeyGravityBool) {
		//Self Explainatory 
		Color32 hex = HexToColor(colorValue);
		
		//find Gravity bool, kinda unneccessary to be its own function 
		bool gravity = grabGravityBool(obeyGravityBool);
		grabPrimitiveType(primitiveType, colorValue, gravity);
		//send values to be created 
		
		
		
	}
	
	
	Color HexToColor(string hex) {
		//HexToColor is from the unity community wiki. Parses Hex returns Color32
		hex.ToUpper();
		if (hex[0] != '#') {
			byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
			byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
			byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
			return new Color32(r, g, b, 255);
		} else {
			byte r = byte.Parse(hex.Substring(1, 2), System.Globalization.NumberStyles.HexNumber);
			byte g = byte.Parse(hex.Substring(3, 2), System.Globalization.NumberStyles.HexNumber);
			byte b = byte.Parse(hex.Substring(5, 2), System.Globalization.NumberStyles.HexNumber);
			return new Color32(r, g, b, 255);
			
		}
		
		
	}
	void grabPrimitiveType(string N, string colorValue, bool gravity) {
		
		Color32 createdColor = HexToColor(colorValue);//set Color
		int xVel = Random.Range(-40, 40); //Random Velocity 
		int yVel = Random.Range(-40, 40);
		int zVel = Random.Range(-40, 40);

		if (objNumber != maxvalue) {//Check if Value is Max
			if (N == "Sphere") {//Check if Sphere 
				
				GameObject objType = GameObject.CreatePrimitive(PrimitiveType.Sphere);//Creates Primitive Sphere
				
				objType.name = "Sphere" + objNumber;//Names it with a increasing int 
				
				
				
				objType.AddComponent < Rigidbody > ();//Components for physics 
				objType.AddComponent < Renderer > ();//for visuals 
				objType.GetComponent < Renderer > ().material.SetColor("_Color", createdColor);//Color
				objType.GetComponent < Rigidbody > ().velocity = new Vector3(xVel, yVel, zVel);//Velocity
				objType.GetComponent < Rigidbody > ().useGravity = gravity;//Gravity 
				
				
				
				objNumber++;//increment number of created values 
				
				
			}
			if (N == "Cube") {
				
				GameObject objType = GameObject.CreatePrimitive(PrimitiveType.Cube);
				objType.name = "Cube" + objNumber;
				
				objType.AddComponent < Rigidbody > ();
				objType.AddComponent < Collider > ();
				objType.AddComponent < Renderer > ();
				objType.GetComponent < Renderer > ().material.SetColor("_Color", createdColor);
				objType.GetComponent < Rigidbody > ().velocity = new Vector3(xVel, yVel, zVel);
				objType.GetComponent < Rigidbody > ().useGravity = gravity;
				
				
				
				
				objNumber++;
				
				
			}
		} else if (maxvalue == objNumber) {
			
			
			objNumber--;//decrease number of objects created 
			Destroy(GameObject.Find(N + (objNumber.ToString())));//Destroy objects 
			
			
			
			
		}
		
		
		
		
	}
	bool grabGravityBool(string N) {
		bool value = false;
		//simple function to find the gravity value
		if (N == "true") {
			value = true;
			
			
		} else if (N == "false") {
			value = false;
		}
		return value;
		
	}
	
	
	
	// Update is called once per frame
	void Update() {
		
	}
}