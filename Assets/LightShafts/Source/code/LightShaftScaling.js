#pragma strict

@script ExecuteInEditMode

//var LightShaftPlane:Transform;
@HideInInspector
var MaxDistance:float=1;
@HideInInspector
var SourceRange:boolean=false;
@HideInInspector
var LightSource:Light;


//Enable color sampling
@HideInInspector
var SampleColor:boolean=false;
//only sample one time
@HideInInspector
var StaticSample:boolean=false;
//set the distance to search for color samples
@HideInInspector
var SampleDistance:float=1;
//Do not edit this color value
@HideInInspector
var SampledColor=Color();

//enable shaft fading and set it up
@HideInInspector
var enableFading:boolean=false;
@HideInInspector
var playerPosition:Transform;
//these variables are only used in this script
@HideInInspector
var playerDistance:float;
var fadeDistance:float;

@HideInInspector
//cull this layer from ray collisions
var hitLayer:LayerMask;
//should the light shaft scale or not?
@HideInInspector
var StaticLightShafts:boolean=false;

private var isCasted:boolean=false;
private var readable:boolean=true;

//used for billboarding
var billBoardOn:boolean=false;
var inverseBillboard:boolean=false;
var offset:float=0;
private var  newRot:Quaternion;
private var cRot:Quaternion;




function Start(){
//get the first rotation and lock it (for billboarding)
cRot = transform.localRotation;


//if static sample only search the color once
if(SampleColor&& StaticSample){
 	ColorSampler();
 	}
 	
	
}               


function Update () {

     //use lightsource light range as ray distance.
     if(SourceRange){
     //if no lightsource is set in slot it will use the lightsource on the transform itself.
     if(!LightSource){
     //if the parent has a lightsource use that
     if(transform.parent && transform.parent.GetComponent(Light)){
          MaxDistance=transform.parent.GetComponent(Light).range;
          }
          //if no parent search for lightsource on the current transform
          else{
          MaxDistance=transform.GetComponent(Light).range;
          }
          }
          //if lightsource is set use that for range.
          else{
        MaxDistance= LightSource.GetComponent.<Light>().range;
          }
     }
     




//get the distance to scale the plane
//dynamic cast
if(!StaticLightShafts){
var hit: RaycastHit;
var RayDirection= transform.forward;

 if(Physics.Raycast(transform.position, RayDirection, hit,MaxDistance,hitLayer)) {
 transform.localScale.z=hit.distance;
 	}
 	else{
 	 transform.localScale.z=MaxDistance;
 	}
 	}
 	else{
//static cast
	if(!isCasted){
	var hit2: RaycastHit;
	var RayDirection2= transform.forward;

	 if(Physics.Raycast(transform.position, RayDirection2, hit2,MaxDistance,hitLayer)) {
	 transform.localScale.z=hit2.distance;
	  		} 
	  		//if no hit detected within range set scale to default maxdistance
	  		else{
			transform.localScale.z=MaxDistance;
	  		}
	  		
 		isCasted=true;
 		}
 	
 	}
 	
 	if(SampleColor&& !StaticSample){
 	ColorSampler();
 	}
  
  if(enableFading){
  	ShaftFader();
 		 }

 		 if(billBoardOn){
 		 	billBoard();
 		 		}

	
}

//Sample color of image behind the current light shaft and send it to the shader.
function ColorSampler(){
var hit: RaycastHit;
var RayDirection= -transform.forward;

  if(Physics.Raycast(transform.position, RayDirection, hit,SampleDistance)) {
  
  //check if mesh has readable colliders
if(hit.collider.GetType() != MeshCollider){
readable=false;
return;
}
else{
readable=true;
}

//use different material setups if in play mode or edit mode
if(readable){
  var TextureMap: Texture2D = hit.transform.GetComponent.<Renderer>().material.mainTexture;
  
  var pixelUV = hit.textureCoord;
  
  var SampledColor = TextureMap.GetPixelBilinear(pixelUV.x, pixelUV.y);
  
  transform.GetComponent.<Renderer>().material.SetColor("_SampleColor",SampledColor);
  }
}
}


function ShaftFader(){
//get the distance per shaft
playerDistance=Vector3.Distance(transform.position,playerPosition.position);
//Multiply it with the fade speed
playerDistance=playerDistance;
//map it to fit within 0 to 1 range.
playerDistance = MathG.Map(playerDistance,0,fadeDistance, 0, 1);
//after mapping to fit within a 0-1 range clamp it
playerDistance = Mathf.Clamp(playerDistance,0,1);

//Debug.Log(playerDistance);

if(playerPosition !=null){
	transform.GetComponent.<Renderer>().material.SetFloat("_ShaftFading",playerDistance);
		}
			}




var waiting:boolean=true;

function billBoard(){
//wait till end of frame before billboarding, so the system can set up its angles.
if(waiting){
yield WaitForEndOfFrame();
waiting=false;

	}


		newRot = Quaternion.LookRotation(playerPosition.position - transform.position);

		//newRot.eulerAngles= new Vector3(newRot.eulerAngles.x + offset, cRot.eulerAngles.y, cRot.eulerAngles.z);
		//newRot.eulerAngles= new Vector3(cRot.eulerAngles.x, newRot.eulerAngles.y + offset, cRot.eulerAngles.z);
		if(!inverseBillboard){
			newRot.eulerAngles= new Vector3(cRot.eulerAngles.x, cRot.eulerAngles.y ,  cRot.eulerAngles.z + ( -newRot.eulerAngles.y + offset) );
				}
					else{
						newRot.eulerAngles= new Vector3(cRot.eulerAngles.x, cRot.eulerAngles.y ,  cRot.eulerAngles.z + ( newRot.eulerAngles.y + offset) );
							}

		transform.localRotation = newRot;
 			   


}
