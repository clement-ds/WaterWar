#pragma strict
@script ExecuteInEditMode

import UnityEngine;
import System.Collections;
import System.Collections.Generic;
 
#if UNITY_EDITOR

import System.IO;
import UnityEditor;
import System.Reflection;
import System.Runtime.Serialization.Formatters.Binary;

#endif

// serialize fase properties of the generator extension
	//show fases
	
	 var Phase1:boolean=false;
	
	 var Phase2:boolean=false;
	
	 var Phase3:boolean=false;

//set the name to write the save setting file
var savePath:String="Generator_preset";

//shaft intensity
var ShaftIntensity:float=0.06;
//linear space
var linearSpace:boolean=false;
private var lnrSpcAjdust:float=2;

//shaft fall off
var ShaftFalloff:float=18;

//shaft color
var ShaftColor: Color=Color(142/255.0F,174/255.0F, 191/255.0F,1);
//shaft warmth

var ShaftWarmth: Color=Color(1,1,1,1);

//This will set the emission intensity and the product of adding shaft color and warmth will be used as emission color
var SoftShaftFactor: float=3;

//sample color behind the shaft and blend it in
var SampleColor:boolean=false;

//Only sample one time
var StaticSample:boolean=false;

//max distance to search for color influence
var SampleDistance:float=3;

//shaft fading
var enableFading:boolean=false;
//reference player position
var playerPosition:Transform;
// set the speed of fading
var fadeDistance:float=10;

//cull this layer from ray collisions
var hitLayer:LayerMask=-1;

//serialize all the layers for use in the editor script (clones the layer list to a string array, this is used to visualize the list in the editor).
var layerList:String[];


//do not apply dynamic scaling

var StaticLightShafts:boolean=false;


//Should we use the transform of a light source as maximum range? This can be the transform itself, if so leave the lightSource slot empty. If false the MaxDistance will be the MaxDistance

var SourceRange:boolean=false;
//use a different light source for maximum range (only if sourceRange is enabled).

var LightSource:Light;

//if none of the above then the ray distance is MaxDistance

var MaxCastDistance:float=30;


var LightShaftMaterial:Material;//Here goes the material for the light shaft system.

var LightShaftMesh:GameObject;//here goes the plane or object used for the shaft's shape

var showShaftWidth:boolean=false;
var ShaftWidth:Vector2=Vector2(0.5,0.5);//the width for each shaft.

//this value is only used for editor serialisation
var showShaftSpace:boolean=false;
//var ShaftSpace:float=0.1;//the spacing between the next spawned shaft.
var ShaftSpace:Vector3= Vector3(0.1,0.1,0);

//this value is only used for editor serialisation
var showShaftRow:boolean=false;
var ShaftRow:Vector3=Vector3(20,20,1);

//only generator preview button can acces this
var CastMesh:boolean=false;

var OutwardCast:boolean=false;

var BackCast:boolean=false;

var SideCast:boolean=false;

var mesh:Mesh;

var MeshScale:float=1;

//only generator preview can button can acces this
var CastCylinder:boolean=false;

//only generator preview can button can acces this
var WallCastXAxis:boolean=false;

//only generator preview can button can acces this
var WallCastYAxis:boolean=false;

//only generator preview can button can acces this
var WallCastZAxis:boolean=false;

//deforms cylinder into a vortex
var Vortex:boolean=false;

var CastCone:boolean=false;

//only generator preview can button can acces this
var CylinderRadius:float=1;

var ConeRadius:float=0;

var ConeOutRadius:float=0.35;



var RandomUpAngle:boolean=false;

var RandomWidthAngle:boolean=false;


var UpAngle:float=32;

var UpSpread:float=0;

var WidthAngle:float=0;

var WidthSpread:float=0;


var Smooth:boolean=true;


var AnimationOn:boolean=true;

var billBoardOn:boolean=false;
var inverseBillboard:boolean=false;
var offset:float=0;



var mobile:boolean=false;

//set the camera's sorting mode
enum SortingMode {Default,Perspective,Orthographic}
var mode : SortingMode = SortingMode.Default;

var cam:List.<Camera> = new List.<Camera>();
var dropdownList:boolean=false;



var DynRot:Vector3 = Vector3(0,0,0.2);





	//awake state is for seting up the cameras their sorting modes.
	function Awake () {

//	if(Application.isPlaying){
	//always have at least one camera in the list


		if(cam.Count <= 0){
			cam.Add (GameObject.FindWithTag("MainCamera").GetComponent.<Camera>());				  			 					  			

				}

	//loop and set each camera in the array
	for(var memberCam:Camera in cam){
		if(memberCam!=null){

			if(mode==SortingMode.Default){
				memberCam.transparencySortMode = TransparencySortMode.Default;
					}


			if(mode==SortingMode.Perspective){
			memberCam.transparencySortMode = TransparencySortMode.Perspective;
					}



			if(mode==SortingMode.Orthographic){
				memberCam.transparencySortMode = TransparencySortMode.Orthographic;
					}

				}
			}
		//}
	}



//Generate shafts according to user settings
function Start () {
 if( Application.isEditor){ 
 DeleteOldShafts();
 
 layerList=GetLayerList().ToArray(String);
 
 }

//check if the application is in play-mode
//if( Application.isPlaying){
DeleteOldShafts();
layerList=GetLayerList().ToArray(String);


//make sure only one cast type is active
if(CastCylinder && mesh){
Debug.LogError("You can't use two cast types at once");
return;
}


if(mesh){
MeshCast();
}
//normal cast
else{
CastSystem();
}


//cast in cylinder shape
if(CastCylinder && !mesh){
CylinderCast();
}


if(CastCone){
ConeCast();
}
if(CastCone && mesh|| CastCylinder && !mesh){
Debug.LogError("please remove mesh from slot if mesh cast is not used");
}
//}

}


function Update(){

 //enable dynamic rotation of the shafts trhough scripting.
 if(AnimationOn){

for(var child:Transform in transform){
    for(var subchild:Transform in child){
		subchild.eulerAngles.x=subchild.eulerAngles.x+ DynRot.x;
		subchild.eulerAngles.y=subchild.eulerAngles.y+ DynRot.y;
		subchild.eulerAngles.z=subchild.eulerAngles.z+ DynRot.z;
		}
  }
  
 }
 /*
 if(billBoardOn){
 for(var child:Transform in transform){
    for(var subchild:Transform in child){

   		 	  var target: Vector3 = playerPosition.position - subchild.position;
   		   		target.z=0;
   		   		target.x=0;

   		   		subchild.LookAt(target);


			}
  				}

	 				}*/


//adjust the uv with falloff and other settings.
 AccurateUV();


}




//this class will generate the light shafts, only use in the Start function, or make sure it only runs once (if in update, etc...)
function CastSystem(){
//if mobile is on always dissable color sampling
if(mobile){

if(SampleColor){
SampleColor=false;
Debug.Log("You can not use real-time color sampling when SharedMaterial/Mobile mode is active. SampleColor is automatically turned off");
}

if(enableFading){
enableFading=false;
Debug.Log("You can not use Shaft Fading when SharedMaterial/Mobile mode is active. Shaft Fading is automatically turned off");

}


}

 //save parents rotation+
   var parentrot:Quaternion= transform.rotation;
   //set rotation to neutral
   transform.rotation=Quaternion.EulerAngles(0,0,0);

//Warnings
if(!LightShaftMaterial){
Debug.LogError("No LightShaftMaterial detected, please set the material to the slot");
return;
}
if(!LightShaftMesh){
Debug.LogError("No LightShaftMesh detected, please set a mesh in the slot");
return;
}
if(SourceRange&&!LightSource){
Debug.LogError("You enabled Source Range but you have not set a LightSource yet");
return;
}



//child on the generator to sub-child the shafts in for mass deletion and refreshing when clicking preview or delete shafts.
var ShaftContainer:GameObject=new GameObject("NewShaft");
//child it under the caster
ShaftContainer.transform.parent=transform;
ShaftContainer.transform.position=transform.position;
ShaftContainer.transform.rotation=transform.rotation;





for(var z:int=0;z<ShaftRow.z;z++){

for(var a:int=0;a<ShaftRow.y;a++){

for(var i:int=0;i<ShaftRow.x;i++){
//instantiate according to loop values 
//set position and rotation

  //the object to spawn
var NewShaft:GameObject = Instantiate (LightShaftMesh, transform.position, Quaternion.identity);



NewShaft.transform.position.x= transform.position.x + i * ShaftSpace.x;
NewShaft.transform.position.y= transform.position.y + a * ShaftSpace.y;
NewShaft.transform.position.z= transform.position.z + z * ShaftSpace.z;
//cast to ground angle
NewShaft.transform.localRotation.eulerAngles.x= UpAngle;
NewShaft.transform.localRotation.eulerAngles.y= WidthAngle;

//if cylinder casting is off, set width spread
if(!CastCylinder){
NewShaft.transform.localRotation.eulerAngles.y= Random.Range(-WidthSpread+WidthAngle/2,WidthSpread+WidthAngle/2);
NewShaft.transform.localRotation.eulerAngles.x= Random.Range(-UpSpread+UpAngle/2,UpSpread+UpAngle/2);
}





//set random rotation angle
if(RandomWidthAngle){
NewShaft.transform.localRotation.eulerAngles.y= Random.rotation.eulerAngles.y;
}


//Set a random tilt angle
if(RandomUpAngle){
NewShaft.transform.localRotation.eulerAngles.x= Random.rotation.eulerAngles.x;
}

if(Smooth){
//Rotate each shaft so it doesn't give a rasterized illusion
NewShaft.transform.localRotation.eulerAngles.z=Random.rotation.eulerAngles.z;
}


//set width
NewShaft.transform.localScale.x=ShaftWidth.x;
NewShaft.transform.localScale.y=ShaftWidth.y;
//add the realtime scaling script and apply the user settings from editor
NewShaft.AddComponent.<LightShaftScaling>();
NewShaft.GetComponent(LightShaftScaling).MaxDistance=MaxCastDistance;
NewShaft.GetComponent(LightShaftScaling).LightSource=LightSource;
NewShaft.GetComponent(LightShaftScaling).SourceRange=SourceRange;
NewShaft.GetComponent(LightShaftScaling).SampleColor=SampleColor;
NewShaft.GetComponent(LightShaftScaling).SampleDistance=SampleDistance;
NewShaft.GetComponent(LightShaftScaling).StaticSample=StaticSample;

NewShaft.GetComponent(LightShaftScaling).enableFading=enableFading;
if(playerPosition !=null){
	NewShaft.GetComponent(LightShaftScaling).playerPosition=playerPosition;
	NewShaft.GetComponent(LightShaftScaling).fadeDistance=fadeDistance;
		}

NewShaft.GetComponent(LightShaftScaling).StaticLightShafts=StaticLightShafts;

//enable billboarding
NewShaft.GetComponent(LightShaftScaling).billBoardOn=billBoardOn;
NewShaft.GetComponent(LightShaftScaling).inverseBillboard=inverseBillboard;
NewShaft.GetComponent(LightShaftScaling).offset=offset;

//Set the cull layers for the shaft's ray cast
NewShaft.GetComponent(LightShaftScaling).hitLayer=hitLayer;

//if not meant for mobile use different material per shaft for dyanmic sampling
if(!mobile){

NewShaft.transform.GetComponent.<Renderer>().material=LightShaftMaterial;


	//compensate intensity when linear space is active
	if(linearSpace){
			NewShaft.transform.GetComponent.<Renderer>().material.SetFloat ("_Intensity", ShaftIntensity/lnrSpcAjdust);
				}
				else{
					NewShaft.transform.GetComponent.<Renderer>().material.SetFloat ("_Intensity", ShaftIntensity);
						}

NewShaft.transform.GetComponent.<Renderer>().material.SetColor ("_ShaftColor", ShaftColor);
NewShaft.transform.GetComponent.<Renderer>().material.SetColor ("_ColorWarmt", ShaftWarmth);
NewShaft.transform.GetComponent.<Renderer>().material.SetFloat ("_InvFade", SoftShaftFactor);


}
else{
NewShaft.transform.GetComponent.<Renderer>().sharedMaterial=LightShaftMaterial;


	if(linearSpace){
			NewShaft.transform.GetComponent.<Renderer>().sharedMaterial.SetFloat ("_Intensity", ShaftIntensity/lnrSpcAjdust);
				}
				else{
					NewShaft.transform.GetComponent.<Renderer>().sharedMaterial.SetFloat ("_Intensity", ShaftIntensity);
						}

NewShaft.transform.GetComponent.<Renderer>().sharedMaterial.SetColor ("_ShaftColor", ShaftColor);
NewShaft.transform.GetComponent.<Renderer>().sharedMaterial.SetColor ("_ColorWarmt", ShaftWarmth);
NewShaft.transform.GetComponent.<Renderer>().sharedMaterial.SetFloat ("_InvFade", SoftShaftFactor);
}




NewShaft.transform.parent=ShaftContainer.transform;


}

}


}

  //reset rotation of parent
   transform.rotation=parentrot;

}






function DeleteOldShafts(){
	 //first clear all old shafts before refreshing
          var tmpCount:int=transform.childCount;
          
          if(tmpCount>=1){
          for (var child : Transform in transform) {
		DestroyImmediate(child.gameObject, false);
		}
		}
}

//creates a cilinder of the shafts after casting
function CylinderCast(){
  //save parents rotation+
   var parentrot:Quaternion= transform.rotation;
   //set rotation to neutral
   transform.rotation=Quaternion.EulerAngles(0,0,0);
   //execute generation
       var i:int=0;
    var AllChilds:int=ShaftRow.x*ShaftRow.y*ShaftRow.z;
    for(var child:Transform in transform){
    for(var subchild:Transform in child){
          i++;
          var angle = i * Mathf.PI * 2 / AllChilds;
          var pos = Vector3 (transform.rotation.y+Mathf.Cos(angle),transform.rotation.z+Mathf.Sin(angle),0) * CylinderRadius;
      	  subchild.position.x=pos.x+child.position.x;
          subchild.position.y=pos.y+child.position.y;
          subchild.position.z=pos.z+child.position.z;
          
           if(Vortex){
          subchild.localRotation=Quaternion.EulerAngles(pos);
          }
          
   }
   }
   //reset rotation of parent
   transform.rotation=parentrot;
}
 
   //sets the shaft to form a cone after casting  
   function ConeCast(){
   //save parents rotation+
   var parentrot:Quaternion= transform.rotation;
   //set rotation to neutral
   transform.rotation=Quaternion.EulerAngles(0,0,0);
   //execute generation
       var i:int=0;
    var AllChilds:int=ShaftRow.x*ShaftRow.y*ShaftRow.z;
    for(var child:Transform in transform){
    for(var subchild:Transform in child){
          i++;
          var angle = i * Mathf.PI * 2 / AllChilds;
          var pos = Vector3 (transform.rotation.y+Mathf.Cos(angle),transform.rotation.z+Mathf.Sin(angle),0) * ConeRadius;
      	  subchild.position.x=pos.x+child.position.x;
          subchild.position.y=pos.y+child.position.y;
          subchild.position.z=child.position.z;
          subchild.localRotation=Quaternion.EulerAngles(Random.Range(-ConeOutRadius/2,ConeOutRadius/2), Random.Range(-ConeOutRadius/2,ConeOutRadius/2), Random.Range(-ConeOutRadius/2,ConeOutRadius/2));
   }
   }
   //reset rotation of parent
   transform.rotation=parentrot;
   }
   
   
   //this class will generate the light shafts on the vertices of a mesh, only use in the Start function, or make sure it only runs once (if in update, etc...)
function MeshCast(){
//if mobile is on always dissable color sampling
if(mobile && SampleColor==true){
SampleColor=false;
Debug.Log("You can not use real-time color sampling when SharedMaterial/Mobile mode is active. SampleColor is automatically turned off");
}

//Warnings
if(!LightShaftMaterial){
Debug.LogError("No LightShaftMaterial detected, please set the material to the slot");
return;
}
if(!LightShaftMesh){
Debug.LogError("No LightShaftMesh detected, please set a mesh in the slot");
return;
}
if(SourceRange&&!LightSource){
Debug.LogError("You enabled Source Range but you have not set a LightSource yet");
return;
}



//child on the generator to sub-child the shafts in for mass deletion and refreshing when clicking preview or delete shafts.
var ShaftContainer:GameObject=new GameObject("NewShaft");
//child it under the caster
ShaftContainer.transform.parent=transform;
ShaftContainer.transform.position=transform.position;


// find all vertices
var vertices : Vector3[] = mesh.vertices;

for (var i = 0; i < mesh.vertexCount-1; i++){

//instantiate according to aount of vertices detected.
//set position and rotation
var pos:Vector3= Vector3(-ShaftContainer.transform.position.x-vertices[i].x*MeshScale,-ShaftContainer.transform.position.y-vertices[i].y*MeshScale,-ShaftContainer.transform.position.z-vertices[i].z*MeshScale);

//the object to spawn
var NewShaft:GameObject = Instantiate (LightShaftMesh, -pos , Quaternion.identity);



//shaft name = vertex number
NewShaft.name=i.ToString();
NewShaft.transform.rotation=ShaftContainer.transform.rotation;



//set rotation to the form the mesh shape
if(OutwardCast){

NewShaft.transform.localRotation.eulerAngles.x= Random.rotation.eulerAngles.x;
NewShaft.transform.localRotation.eulerAngles.y= Random.rotation.eulerAngles.y;

}


if(BackCast){

NewShaft.transform.rotation.x=ShaftContainer.transform.rotation.x;
NewShaft.transform.rotation.y=ShaftContainer.transform.rotation.y;

}

if(SideCast){

NewShaft.transform.LookAt(-ShaftContainer.transform.position-vertices[i]);

}


if(!OutwardCast && !BackCast && !SideCast){
Debug.LogError("Please Select a sub type, BackCast selected as scene filler");
}

//set random rotation angle
if(RandomWidthAngle && !OutwardCast){
NewShaft.transform.localRotation.eulerAngles.y= Random.rotation.eulerAngles.y;
}


//Set a rondom tilt angle
if(RandomUpAngle && !OutwardCast){
NewShaft.transform.localRotation.eulerAngles.x= Random.rotation.eulerAngles.x;
}
//if above is false set a fixed angle
if(!RandomUpAngle && !OutwardCast){
//casting angle
NewShaft.transform.localRotation.eulerAngles.x= UpAngle;
NewShaft.transform.localRotation.eulerAngles.y= WidthAngle;
//casting angle spread
NewShaft.transform.localRotation.eulerAngles.y= Random.Range(-WidthSpread+WidthAngle/2,WidthSpread+WidthAngle/2);
NewShaft.transform.localRotation.eulerAngles.x= Random.Range(-UpSpread+UpAngle/2,UpSpread+UpAngle/2);
}

if(Smooth){
//Rotate each shaft so it doesn't give a rasterized illusion
NewShaft.transform.localRotation.eulerAngles.z=Random.rotation.eulerAngles.z;
}


//set width
NewShaft.transform.localScale.x=ShaftWidth.x;
NewShaft.transform.localScale.y=ShaftWidth.y;

//add the realtime scaling script and apply the user settings from editor
NewShaft.AddComponent.<LightShaftScaling>();
NewShaft.GetComponent(LightShaftScaling).MaxDistance=MaxCastDistance;
NewShaft.GetComponent(LightShaftScaling).LightSource=LightSource;
NewShaft.GetComponent(LightShaftScaling).SourceRange=SourceRange;
NewShaft.GetComponent(LightShaftScaling).SampleColor=SampleColor;
NewShaft.GetComponent(LightShaftScaling).SampleDistance=SampleDistance;
NewShaft.GetComponent(LightShaftScaling).StaticSample=StaticSample;

NewShaft.GetComponent(LightShaftScaling).enableFading=enableFading;
if(playerPosition !=null){
	NewShaft.GetComponent(LightShaftScaling).playerPosition=playerPosition;
	NewShaft.GetComponent(LightShaftScaling).fadeDistance=fadeDistance;

		}

NewShaft.GetComponent(LightShaftScaling).StaticLightShafts=StaticLightShafts;

//enable billboarding
NewShaft.GetComponent(LightShaftScaling).billBoardOn=billBoardOn;
NewShaft.GetComponent(LightShaftScaling).inverseBillboard=inverseBillboard;
NewShaft.GetComponent(LightShaftScaling).offset=offset;

//set cull layer for shaft ray
NewShaft.GetComponent(LightShaftScaling).hitLayer=hitLayer;

//if not meant for mobile use different material per shaft for dyanmic sampling
if(!mobile){
NewShaft.transform.GetComponent.<Renderer>().material=LightShaftMaterial;

	//compensate intensity when linear space is active
	if(linearSpace){
			NewShaft.transform.GetComponent.<Renderer>().material.SetFloat ("_Intensity", ShaftIntensity/lnrSpcAjdust);
				}
				else{
					NewShaft.transform.GetComponent.<Renderer>().material.SetFloat ("_Intensity", ShaftIntensity);
						}

NewShaft.transform.GetComponent.<Renderer>().material.SetColor ("_ShaftColor", ShaftColor);
NewShaft.transform.GetComponent.<Renderer>().material.SetColor ("_ColorWarmt", ShaftWarmth);
NewShaft.transform.GetComponent.<Renderer>().material.SetFloat ("_InvFade", SoftShaftFactor);


}
else{
NewShaft.transform.GetComponent.<Renderer>().sharedMaterial=LightShaftMaterial;


	if(linearSpace){
			NewShaft.transform.GetComponent.<Renderer>().sharedMaterial.SetFloat ("_Intensity", ShaftIntensity/lnrSpcAjdust);
				}
				else{
					NewShaft.transform.GetComponent.<Renderer>().sharedMaterial.SetFloat ("_Intensity", ShaftIntensity);
						}

NewShaft.transform.GetComponent.<Renderer>().sharedMaterial.SetColor ("_ShaftColor", ShaftColor);
NewShaft.transform.GetComponent.<Renderer>().sharedMaterial.SetColor ("_ColorWarmt", ShaftWarmth);
NewShaft.transform.GetComponent.<Renderer>().sharedMaterial.SetFloat ("_InvFade", SoftShaftFactor);
}



NewShaft.transform.parent=ShaftContainer.transform;



}
ShaftContainer.transform.rotation=transform.rotation;
}


function AccurateUV(){

for(var child:Transform in transform){
    for(var subchild:Transform in child){
//if not meant for mobile use different material per shaft for dyanmic sampling
 if(!mobile){
  subchild.GetComponent.<Renderer>().material.SetTextureScale ("_MainTex", Vector2(1,subchild.localScale.z/ShaftFalloff));
  }
  else{
  subchild.GetComponent.<Renderer>().sharedMaterial.SetTextureScale ("_MainTex", Vector2(1,subchild.localScale.z/ShaftFalloff));
  }


}
}

}


function GetLayerList():ArrayList{

  var layerNames :ArrayList= new ArrayList();
//user defined layers start with layer 8 and unity supports 31 layers
 for(var i:int =0;i<=31;i++){
 
 //get the name of the layer
   var layerN=LayerMask.LayerToName(i); 
   
 //only add the layer if it has been named 
  // if(layerN.Length>0) 
     layerNames.Add(layerN);
 }

return layerNames;

}




#if UNITY_EDITOR

   //draw the gizmo
 function OnDrawGizmos () {
 
 //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//if gizmo folder does not exist create it
if (!Directory.Exists(Application.dataPath+"/Gizmos")){
Directory.CreateDirectory(Application.dataPath+"/Gizmos");
}
//then copy the gizmo into the folder
var info = new DirectoryInfo(Application.dataPath+"/Gizmos");
var fileInfo = info.GetFiles();
if(!File.Exists(Application.dataPath+"/Gizmos/LightShaftGizmo.tif")){
File.Copy(Application.dataPath+"/LightShafts/Source/Resources/UI/LightShaftGizmo.tif",Application.dataPath+"/Gizmos/LightShaftGizmo.tif");
}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////	
 
		Gizmos.DrawIcon (transform.position, "LightShaftGizmo.tif", true);
	}
	
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////	
//save all variables/settings to a file
function saveSettings(){
//catch the path in an editor window dialogue
	var path = EditorUtility.SaveFilePanel("Save preset",Application.dataPath+"/LightShafts/Presets",transform.name,"lsgp");

			if(path.Length != 0) {

			//set the path
			savePath=path;


			//check if the current path already exists
				if(File.Exists(savePath)){
				if(EditorUtility.DisplayDialog("Overwrite preset?", "You are about to overwrite an existing preset: " + savePath + "Are you sure you want to save?", "yes, I am sure!", "No! Glad you double checked.")){
					SaveSequince();
						}
					}
					//if not save straight away
					else{
					SaveSequince();
						}

	
			}

}

	function SaveSequince(){
	//create the formatter and the write file location
	var bf: BinaryFormatter = new BinaryFormatter();
	var file:FileStream = File.Create(savePath);

		//get the list of save variables in GeneratorData class (this list ist he same as the main variables on top)
		var data:GeneratorData = new GeneratorData();
     				  

		//set the values to be saved here
		data.ShaftIntensity=ShaftIntensity;
		data.linearSpace=linearSpace;
		data.ShaftFalloff=ShaftFalloff;
	
		data.SoftShaftFactor=SoftShaftFactor;
		data.SampleColor=SampleColor;
		data.StaticSample=StaticSample;
		data.SampleDistance=SampleDistance;

		data.enableFading=enableFading;
		data.fadeDistance=fadeDistance;


		data.layerList=layerList;
		
		data.StaticLightShafts=StaticLightShafts;
		data.SourceRange=SourceRange;
		
		data.MaxCastDistance=MaxCastDistance;

		data.showShaftWidth=showShaftWidth;
		data.ShaftWidthX=ShaftWidth.x;
		data.ShaftWidthY=ShaftWidth.y;

		data.showShaftSpace=showShaftSpace;
		data.ShaftSpaceX=ShaftSpace.x;
		data.ShaftSpaceY=ShaftSpace.y;
		data.ShaftSpaceZ=ShaftSpace.z;

		data.showShaftRow=showShaftRow;
		data.ShaftRowX=ShaftRow.x;
		data.ShaftRowY=ShaftRow.y;
		data.ShaftRowZ=ShaftRow.z;

		data.CastMesh=CastMesh;
		data.OutwardCast=OutwardCast;
		data.BackCast=BackCast;
		data.SideCast=SideCast;
		
		data.MeshScale=MeshScale;
		data.CastCylinder=CastCylinder;
		data.WallCastXAxis=WallCastXAxis;
		data.WallCastYAxis=WallCastYAxis;
		data.WallCastZAxis=WallCastZAxis;
		data.Vortex=Vortex;
		data.CastCone=CastCone;
		data.CylinderRadius=CylinderRadius;
		data.ConeRadius=ConeRadius;
		data.ConeOutRadius=ConeOutRadius;
		data.RandomUpAngle=RandomUpAngle;
		data.UpAngle=UpAngle;
		data.UpSpread=UpSpread;
		data.WidthAngle=WidthAngle;
		data.WidthSpread=WidthSpread;
		data.Smooth=Smooth;

		data.AnimationOn=AnimationOn;
		data.billBoardOn=billBoardOn;
		data.inverseBillboard=inverseBillboard;
		data.offset=offset;

		data.mobile=mobile;

		data.mode=mode;
		data.dropdownList=dropdownList;

		//here we treat the special values such as color and vector3.
		//we have to split each channel/vector into floats
		data.ShaftColorR=ShaftColor.r;
		data.ShaftColorG=ShaftColor.g;
		data.ShaftColorB=ShaftColor.b;
		data.ShaftColorA=ShaftColor.a;
		
		data.ShaftWarmthR=ShaftWarmth.r;
		data.ShaftWarmthG=ShaftWarmth.g;
		data.ShaftWarmthB=ShaftWarmth.b;
		data.ShaftWarmthA=ShaftWarmth.a;
		
		data.DynRotX = DynRot.x;
		data.DynRotY = DynRot.y;
		data.DynRotZ = DynRot.z;
		
			
			//now that the values in the class are set to the current inspector vlalues, serialize the data class
			bf.Serialize(file,data);
			//when done writing, close the file
			file.Close();
		
Debug.Log("settings saved to file: "+ savePath);
	}




			
//load all variables/settings from a file
function loadSettings(){

//catch the path in an editor window dialogue
	var path = EditorUtility.OpenFilePanel("Open preset",Application.dataPath+"/LightShafts/Presets","lsgp");

		//set the selected path to load
		savePath=path;

			if(savePath.Length != 0) {

//first check if the requested file actually exists in the folder
	if(File.Exists(savePath)){

	//ask one more time to be sure the user wants to load this preset file.
	if( EditorUtility.DisplayDialog("Load preset?", "Are you sure you want to load preset: " + savePath + " in this generator?", "yes, load it up!", "No!")){

	var bf: BinaryFormatter = new BinaryFormatter();
	var file:FileStream = File.Open(savePath,FileMode.Open);
	//find the GeneratorData class in the file and deserialize it
	var data:GeneratorData = bf.Deserialize(file) as GeneratorData;
	//when deserialized close the file again.
	file.Close();	
		
		//set the values to be loaded variables here
		ShaftIntensity=data.ShaftIntensity;
		linearSpace=data.linearSpace;
		ShaftFalloff=data.ShaftFalloff;
	
		SoftShaftFactor=data.SoftShaftFactor;
		SampleColor=data.SampleColor;
		StaticSample=data.StaticSample;
		SampleDistance=data.SampleDistance;

		enableFading=data.enableFading;
		fadeDistance=data.fadeDistance;

		
		layerList=data.layerList;
		
		StaticLightShafts=data.StaticLightShafts;
		SourceRange=data.SourceRange;
		
		MaxCastDistance=data.MaxCastDistance;

		showShaftWidth=data.showShaftWidth;
		ShaftWidth.x=data.ShaftWidthX;
		ShaftWidth.y=data.ShaftWidthY;

		showShaftSpace=data.showShaftSpace;
		ShaftSpace.x=data.ShaftSpaceX;
		ShaftSpace.y=data.ShaftSpaceY;
		ShaftSpace.z=data.ShaftSpaceZ;

		showShaftRow=data.showShaftRow;
		ShaftRow.x=data.ShaftRowX;
		ShaftRow.y=data.ShaftRowY;
		ShaftRow.z=data.ShaftRowZ;
		
		CastMesh=data.CastMesh;
		OutwardCast=data.OutwardCast;
		BackCast=data.BackCast;
		SideCast=data.SideCast;
		
		MeshScale=data.MeshScale;
		CastCylinder=data.CastCylinder;
		
		WallCastXAxis=data.WallCastXAxis;
		WallCastYAxis=data.WallCastYAxis;
		WallCastZAxis=data.WallCastZAxis;
		
		Vortex=data.Vortex;
		
		CastCone=data.CastCone;
		CylinderRadius=data.CylinderRadius;
		ConeRadius=data.ConeRadius;
		ConeOutRadius=data.ConeOutRadius;
		
		RandomUpAngle=data.RandomUpAngle;
		UpAngle=data.UpAngle;
		UpSpread=data.UpSpread;
		WidthAngle=data.WidthAngle;
		WidthSpread=data.WidthSpread;
		Smooth=data.Smooth;

		AnimationOn=data.AnimationOn;
		billBoardOn=data.billBoardOn;
		inverseBillboard=data.inverseBillboard;
		offset=data.offset;

		mobile=data.mobile;

		mode=data.mode;
		dropdownList=data.dropdownList;


		//here we treat the special values such as color and vector3.
		//we have to split each channel/vector into floats
		ShaftColor.r=data.ShaftColorR;
		ShaftColor.g=data.ShaftColorG;
		ShaftColor.b=data.ShaftColorB;
		ShaftColor.a=data.ShaftColorA;
		
		ShaftWarmth.r=data.ShaftWarmthR;
		ShaftWarmth.g=data.ShaftWarmthG;
		ShaftWarmth.b=data.ShaftWarmthB;
		ShaftWarmth.a=data.ShaftWarmthA;
		
		DynRot.x=data.DynRotX;
		DynRot.y=data.DynRotY;
		DynRot.z=data.DynRotZ;
		
				
		
Debug.Log("loaded settings: "+savePath);

}
			}

	}
}

//this class has the exact same variables as the script originally has.
//these values will be set identically to their counter part and written to a file.
//the reverse process is used during load.
class GeneratorData {


var ShaftIntensity:float;
var linearSpace:boolean;
var ShaftFalloff:float;

//colors will be split in floats
//var ShaftColor: Color;
  var ShaftColorR:float;
  var ShaftColorG:float;
  var ShaftColorB:float;
  var ShaftColorA:float;

//colors will be split in floats
//var ShaftWarmth: Color;
  var ShaftWarmthR:float;
  var ShaftWarmthG:float;
  var ShaftWarmthB:float;
  var ShaftWarmthA:float;


var SoftShaftFactor: float;

var SampleColor:boolean;
var StaticSample:boolean;
var SampleDistance:float;

var enableFading:boolean;
var fadeDistance:float;

//var hitLayer:LayerMask;
var layerList:String[];


var StaticLightShafts:boolean;


var SourceRange:boolean;
//var LightSource:Light;

var MaxCastDistance:float;

//we do not need these as they are preset on start
//var LightShaftMaterial:Material;
//var LightShaftMesh:GameObject;

var showShaftWidth:boolean;
var ShaftWidthX:float;
var ShaftWidthY:float;

var showShaftSpace:boolean;
var ShaftSpaceX:float;
var ShaftSpaceY:float;
var ShaftSpaceZ:float;

var showShaftRow:boolean;
var ShaftRowX:int;
var ShaftRowY:int;
var ShaftRowZ:int;




var CastMesh:boolean;
var OutwardCast:boolean;
var BackCast:boolean;
var SideCast:boolean;
//var mesh:Mesh;
var MeshScale:float;
var CastCylinder:boolean;
var WallCastXAxis:boolean;
var WallCastYAxis:boolean;
var WallCastZAxis:boolean;
var Vortex:boolean;
var CastCone:boolean;
var CylinderRadius:float;
var ConeRadius:float;
var ConeOutRadius:float;


var RandomUpAngle:boolean;
var RandomWidthAngle:boolean;

var UpAngle:float;
var UpSpread:float;
var WidthAngle:float;
var WidthSpread:float;

var Smooth:boolean;

var AnimationOn:boolean;
var billBoardOn:boolean;
var inverseBillboard:boolean;
var offset:float;

var mobile:boolean;

//enum SortingMode {Default,Perspective,Orthographic}
var mode : SortingMode;
//var cam:List.<Camera>;
var dropdownList:boolean=false;

//vector 3s will be split in floats
//var DynRot:Vector3;
var DynRotX:float;
var DynRotY:float;
var DynRotZ:float;


/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//backwards compatibility of older preset files. These variables no longer exist within the system, but are placed here for catch purposes.

var ShaftWidth:float;
var ShaftSpace:float;

}
	
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////	
	
	
	#endif