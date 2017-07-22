#pragma strict
//import System;
//import System.IO;
import UnityEngine;
import UnityEditor;

	@CustomEditor(LightShaftGenerator)
	


 
//@System.Serializable
class GeneratorExtension extends Editor  {
	

	
	
	
	function Start(){
		
		
		
	 DeleteOldShafts();
	}
	
		//apply only to this generator
		 var CurrentGenerator = Selection.activeGameObject;
		 
		
		 var deleteTex:Texture;
		 var previewTex:Texture;
		 var saveTex:Texture;
		 var loadTex:Texture;
			
/////////////////////////////////////////////////////////////////////////////////////////////////		
		function OnInspectorGUI() {
		//Hide normal transform tools (we do not need scale)
		 CurrentGenerator.transform.hideFlags = HideFlags.HideInInspector;
		
	//load textures
	if(deleteTex==null||previewTex==null||saveTex==null||saveTex==loadTex){
		LoadText();
			}
		
//--General generator info--//	

		//show number of shafts on this generator
		var ShaftCounter:int=0;
		var LoadingString:String="";
		
		//show number of verts on the parent mesh.
		var VertCounter:int=0;

		//create a vector 3 for rotation to cast the quaternion in.
		var genRotation=Vector3 (CurrentGenerator.transform.rotation.x,CurrentGenerator.transform.rotation.y,CurrentGenerator.transform.rotation.z);

		//lock the size
		CurrentGenerator.transform.localScale = Vector3(1,1,1);

		//now set and reference both to the transform.
		CurrentGenerator.transform.position = EditorGUILayout.Vector3Field(GUIContent ("position",""), CurrentGenerator.transform.position);
		genRotation = EditorGUILayout.Vector3Field(GUIContent ("rotation",""), genRotation);
		GUILayout.Label("Scale: (locked) \n Use Phase 2 -> Shaft width and Shaft space \n to properly change the size of the generator.");
GUILayout.Label("");

		Repaint();


	//	CurrentGenerator.transform.position=genPosition;
	//	CurrentGenerator.transform.rotation=genRotation;


		// shaft counter
		  for (var child : Transform in CurrentGenerator.transform.transform) {
  			 for(var subChild: Transform in child){ 
  			 ShaftCounter++;
  			 }
  			 }
  			 
  			 //count shafts and show load process.
		//GUILayout.Label("Shafts casted "+ ShaftCounter);
		if(ShaftCounter==0){
		LoadingString="No shafts casted";
		}

	if(ShaftCounter>1&&ShaftCounter>ShaftCounter-1){
	LoadingString="Shafts casted: "+ ShaftCounter;
	}
	
	//if not mobile use normal progress bar else change to mobile counting
	var percentage:float=MathG.Percentage(ShaftCounter,1200,false);
	var percentageMobile:float=MathG.Percentage(ShaftCounter,2400,false);
	
	if(!CurrentGenerator.GetComponent(LightShaftGenerator).mobile){
		ProgressBar (percentage/100, LoadingString);
		}
		else{
		ProgressBar (percentageMobile/100, LoadingString);
		
		}
		
		//vert counter
		// find all vertices
		if(CurrentGenerator.GetComponent(LightShaftGenerator).CastMesh){
		var mesh : Mesh = CurrentGenerator.GetComponent(LightShaftGenerator).mesh;
		if(!mesh){
		Debug.LogError("Warning! No mesh detected, please insert mesh.");
		}
		if(mesh){
		VertCounter = mesh.vertexCount;
		
		}
		}
		
/////////////////////////////////////////////////////////////////////////////////////////////////////		
//-----save/load buttons------//
GUILayout.BeginHorizontal();	


GUILayout.Label(GUIContent ("Safe/Load presets:","If you like the current settings of this Generator or you want to share them with friends. You can save your current setup as a .lsgp preset file."));
GUILayout.Label("");
GUILayout.Label("");
GUILayout.Label("");
GUILayout.Label(GUIContent ("Preview:","Render current shaft setup within editor mode."));


GUILayout.EndHorizontal();	

GUILayout.BeginHorizontal();		

		  //activate save or load function.
	if(GUILayout.Button(GUIContent (loadTex,"Load settings from a file "),GUILayout.Width(64),GUILayout.Height(64))) {
		 CurrentGenerator.GetComponent(LightShaftGenerator).loadSettings();
		 
			 CurrentGenerator.GetComponent(LightShaftGenerator).Start();
					 SceneView.RepaintAll();

 		}

 				if(GUILayout.Button(GUIContent(saveTex,"Save current settings to file. Except for these variables: LightSource, LightShaftMaterial, LightShaftMesh, mesh. These variables are non serializable."),GUILayout.Width(64),GUILayout.Height(64))) {
			CurrentGenerator.GetComponent(LightShaftGenerator).saveSettings();
				}

//-----preview buttons------//	
GUILayout.Label("");
GUILayout.Label("");
GUILayout.Label("");

		if(GUILayout.Button(GUIContent(deleteTex,"Delete all shafts on this generator"),GUILayout.Width(32),GUILayout.Height(32))) {
	DeleteOldShafts();
	}

	   //generate in editor preview
		 if(GUILayout.Button(GUIContent (previewTex,"Preview current settings. Note that sometimes, shafts are rendered with the wrong falloff. This is due to the editor view not being realtime. Simply hover over a button to get its tooltip, this will refresh the scene view and fix the shafts. There is nothing I can do on my end to fix this issue. "),GUILayout.Width(64),GUILayout.Height(64))) {
		 CurrentGenerator.GetComponent(LightShaftGenerator).Start();
 		 SceneView.RepaintAll();
 		}

 		GUILayout.EndHorizontal();
 		
 		
 		
		GUILayout.Label("");
		
	
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////			
///Required Setup
	GUI.contentColor = Color(255,100,0);
	GUILayout.Label(GUIContent ("REQUIRED:","This MUST be setup first before you cast. An error will be thrown out if you forgot or if you have placed a non castable GameObject inside the Transform slot"));
	GUI.contentColor = Color.white;
//Redirect material to generator
CurrentGenerator.GetComponent(LightShaftGenerator).LightShaftMaterial = EditorGUILayout.ObjectField(CurrentGenerator.GetComponent(LightShaftGenerator).LightShaftMaterial , Material, false);
//Redirect mesh to generator
CurrentGenerator.GetComponent(LightShaftGenerator).LightShaftMesh = EditorGUILayout.ObjectField(CurrentGenerator.GetComponent(LightShaftGenerator).LightShaftMesh , GameObject, false);			

											
		GUILayout.Label("");

///////////////////////////////////////////////////////////////////////////////////////////////	///////////////////////////////////////////////////////////////////////////////////////////////
//-----Phase 1 setup-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//		
	CurrentGenerator.GetComponent(LightShaftGenerator).Phase1 = EditorGUILayout.Foldout(CurrentGenerator.GetComponent(LightShaftGenerator).Phase1, GUIContent ("Phase 1 setup", "Contains the system's cast types."));
	if(CurrentGenerator.GetComponent(LightShaftGenerator).Phase1){
		GUILayout.Label("------------------------------------------------------------------------------");
			GUILayout.Label("Select cast type ");
			GUILayout.Label("None = default Square cast");
		
		//toggle the cast types
		//mesh casting
		CurrentGenerator.GetComponent(LightShaftGenerator).CastMesh=GUILayout.Toggle(CurrentGenerator.GetComponent(LightShaftGenerator).CastMesh, GUIContent ("Mesh cast", "Gather data to cast a shaft for every vertex found. This wil not place an object in the scene, it only reads in the position of each vertex point. Overrides: Shaft row x,y,z and Shaft space"));
		if(CurrentGenerator.GetComponent(LightShaftGenerator).CastMesh){
		GUILayout.Label("Place mesh for mesh casting (Overrides shaft space and row value.");
		GUILayout.Label("(amount of casted shafts = amount of vertices)");
		CurrentGenerator.GetComponent(LightShaftGenerator).mesh = EditorGUILayout.ObjectField(CurrentGenerator.GetComponent(LightShaftGenerator).mesh , Mesh, false);
		
		//set Scale
		CurrentGenerator.GetComponent(LightShaftGenerator).MeshScale = EditorGUILayout.FloatField("MeshScale:", CurrentGenerator.GetComponent(LightShaftGenerator).MeshScale);
		GUILayout.Label(" ");
		//outward cast
		CurrentGenerator.GetComponent(LightShaftGenerator).OutwardCast=GUILayout.Toggle(CurrentGenerator.GetComponent(LightShaftGenerator).OutwardCast,"Cast outward");
			if(CurrentGenerator.GetComponent(LightShaftGenerator).OutwardCast){
			GUILayout.Label("(Overrides Random Up, Width, Fixed Angle and Shaft space)");
			GUILayout.Label(" ");
			CurrentGenerator.GetComponent(LightShaftGenerator).BackCast=false;
			CurrentGenerator.GetComponent(LightShaftGenerator).SideCast=false;
		}
		
		//BackCasting
		CurrentGenerator.GetComponent(LightShaftGenerator).BackCast=GUILayout.Toggle(CurrentGenerator.GetComponent(LightShaftGenerator).BackCast,"Cast from behind");
			if(CurrentGenerator.GetComponent(LightShaftGenerator).BackCast){
			GUILayout.Label("(All functions can be applied, except Shaft space)");
			GUILayout.Label(" ");
			CurrentGenerator.GetComponent(LightShaftGenerator).OutwardCast=false;
			CurrentGenerator.GetComponent(LightShaftGenerator).SideCast=false;
		}
		
		//SideCasting
			CurrentGenerator.GetComponent(LightShaftGenerator).SideCast=GUILayout.Toggle(CurrentGenerator.GetComponent(LightShaftGenerator).SideCast,"Cast from the side");
			if(CurrentGenerator.GetComponent(LightShaftGenerator).SideCast){
			GUILayout.Label("(All functions can be applied, except Shaft space)");
			GUILayout.Label(" ");
			CurrentGenerator.GetComponent(LightShaftGenerator).OutwardCast=false;
			CurrentGenerator.GetComponent(LightShaftGenerator).BackCast=false;
		}
	
		
		
		
		//count vertices
		GUILayout.Label("Verts detected on mesh "+ VertCounter);
		
		GUILayout.Label("This is the raw vertex count as the gpu sees it");
		GUILayout.Label("and may differ from your 3D application");
		GUILayout.Label(" ");
		
		CurrentGenerator.GetComponent(LightShaftGenerator).CastCylinder=false;
		}
		else{
		CurrentGenerator.GetComponent(LightShaftGenerator).mesh = null;
		CurrentGenerator.GetComponent(LightShaftGenerator).OutwardCast=false;
			CurrentGenerator.GetComponent(LightShaftGenerator).BackCast=false;
			CurrentGenerator.GetComponent(LightShaftGenerator).SideCast=false;
		}
		//Cone casting
		CurrentGenerator.GetComponent(LightShaftGenerator).CastCone=GUILayout.Toggle(CurrentGenerator.GetComponent(LightShaftGenerator).CastCone,GUIContent ("Cone cast", "Collects all the casted shafts and places them in a cone shape. Overrides: shaft space, up angle/spread, width angle/spread, random up/width angle and smooth (does this by default)"));
		if(CurrentGenerator.GetComponent(LightShaftGenerator).CastCone){
		
		GUILayout.Label("Radius overrides shaft space value");
		GUILayout.Label("(amount of shafts is the multiplication of Row x*y*z)");
		
		CurrentGenerator.GetComponent(LightShaftGenerator).CastCylinder=false;
		CurrentGenerator.GetComponent(LightShaftGenerator).CastMesh=false;
		
		//set radius
		CurrentGenerator.GetComponent(LightShaftGenerator).ConeRadius = EditorGUILayout.FloatField("Inner radius:", CurrentGenerator.GetComponent(LightShaftGenerator).ConeRadius);
		CurrentGenerator.GetComponent(LightShaftGenerator).ConeOutRadius = EditorGUILayout.FloatField("Outer radius:", CurrentGenerator.GetComponent(LightShaftGenerator).ConeOutRadius);
		}
		
		//cylinder casting
		CurrentGenerator.GetComponent(LightShaftGenerator).CastCylinder=GUILayout.Toggle(CurrentGenerator.GetComponent(LightShaftGenerator).CastCylinder,GUIContent ("Cylinder cast","Collects all shafts and places them in a cylinder. Overrides: shaft space, up/width spread"));
		
		if(CurrentGenerator.GetComponent(LightShaftGenerator).CastCylinder){
		GUILayout.Label("Radius overrides shaft space value");
		GUILayout.Label("(amount of shafts is the multiplication of Row x*y*z)");
		
		CurrentGenerator.GetComponent(LightShaftGenerator).Vortex = GUILayout.Toggle(CurrentGenerator.GetComponent(LightShaftGenerator).Vortex,"Turn cylinder in vortex");
		//set the radius
		CurrentGenerator.GetComponent(LightShaftGenerator).CylinderRadius = EditorGUILayout.FloatField("Radius:", CurrentGenerator.GetComponent(LightShaftGenerator).CylinderRadius);
		
		CurrentGenerator.GetComponent(LightShaftGenerator).CastMesh=false;
		CurrentGenerator.GetComponent(LightShaftGenerator).CastCone=false;
	 	}
		else{
		//vortex cast
		CurrentGenerator.GetComponent(LightShaftGenerator).Vortex = false;
			
		}

//use animation 
 GUILayout.Label("");
 GUILayout.Label("Animation");
CurrentGenerator.GetComponent(LightShaftGenerator).AnimationOn=GUILayout.Toggle(CurrentGenerator.GetComponent(LightShaftGenerator).AnimationOn,GUIContent ("Auto shaft rotation","ENABLE for scriptable rotation! This will let you set the rotation speed for each shaft's local rotaion. This replaces the old 'file' dependant animation system. This native supported animation will be much more performance friendly."));
			if(CurrentGenerator.GetComponent(LightShaftGenerator).AnimationOn){

			 CurrentGenerator.GetComponent(LightShaftGenerator).billBoardOn=false;

			CurrentGenerator.GetComponent(LightShaftGenerator).DynRot = EditorGUILayout.Vector3Field("Rotation speed:", CurrentGenerator.GetComponent(LightShaftGenerator).DynRot);
			}

CurrentGenerator.GetComponent(LightShaftGenerator).billBoardOn=GUILayout.Toggle(CurrentGenerator.GetComponent(LightShaftGenerator).billBoardOn,GUIContent ("Billboard","Each shaft will always face towardsthe camera."));
			if(CurrentGenerator.GetComponent(LightShaftGenerator).billBoardOn){

				CurrentGenerator.GetComponent(LightShaftGenerator).AnimationOn=false;

			GUILayout.BeginHorizontal();
			GUILayout.Label("Player position");
			CurrentGenerator.GetComponent(LightShaftGenerator).playerPosition = EditorGUILayout.ObjectField(CurrentGenerator.GetComponent(LightShaftGenerator).playerPosition , Transform, true);
			GUILayout.EndHorizontal();

			CurrentGenerator.GetComponent(LightShaftGenerator).inverseBillboard=GUILayout.Toggle(CurrentGenerator.GetComponent(LightShaftGenerator).inverseBillboard,GUIContent ("Inverse billboard","In case the shafts rotate in the wrong direction, this option will inverse it. (Cone cast for example needs this option).."));
			CurrentGenerator.GetComponent(LightShaftGenerator).offset = EditorGUILayout.FloatField("rotation offset:", CurrentGenerator.GetComponent(LightShaftGenerator).offset);


					}
GUILayout.Label("------------------------------------------------------------------------------");
		
		}
		
		
//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//						
////////////////////////////////////////////////////////////
//-----Phase 2 setup-----//		
GUILayout.Label("");
CurrentGenerator.GetComponent(LightShaftGenerator).Phase2 = EditorGUILayout.Foldout(CurrentGenerator.GetComponent(LightShaftGenerator).Phase2, GUIContent ("Phase 2 setup", "Contains the shafts's color, range/collision and cast settings"));	
if(CurrentGenerator.GetComponent(LightShaftGenerator).Phase2){

GUILayout.Label("_________________________________________________________");
GUILayout.Label("Color settings:");

//Redirect shaft intensity to generator
CurrentGenerator.GetComponent(LightShaftGenerator).ShaftIntensity = EditorGUILayout.Slider(GUIContent ("Shaft Intensity:","How bright should the shafts be?"),CurrentGenerator.GetComponent(LightShaftGenerator).ShaftIntensity,0.0,1.0);
				CurrentGenerator.GetComponent(LightShaftGenerator).linearSpace=GUILayout.Toggle(CurrentGenerator.GetComponent(LightShaftGenerator).linearSpace,GUIContent ("Linear space adjust","If the project is in Linear color space enable this to compensate for the value shifts. /n Note that the colors in Linear space will always look slightly different compared to Gamma space"));


//Redirect shaft falloff to generator
CurrentGenerator.GetComponent(LightShaftGenerator).ShaftFalloff = EditorGUILayout.Slider(GUIContent ("Shaft Falloff:","How sharp should the shaft collide against an object: 0.001 super soft, max = extremely sharp"), CurrentGenerator.GetComponent(LightShaftGenerator).ShaftFalloff,0.1,CurrentGenerator.GetComponent(LightShaftGenerator).MaxCastDistance);


GUILayout.Label("");

//Redirect color values to generator
CurrentGenerator.GetComponent(LightShaftGenerator).ShaftColor = EditorGUILayout.ColorField(GUIContent ("Shaft Color:","The main color of the shaft"), CurrentGenerator.GetComponent(LightShaftGenerator).ShaftColor);
CurrentGenerator.GetComponent(LightShaftGenerator).ShaftWarmth = EditorGUILayout.ColorField(GUIContent("Shaft Warmth:","This color will be combined with the Shaft Color to give it extra tone, red = warmer, blue= colder, this is of course not only limited to those two colors."), CurrentGenerator.GetComponent(LightShaftGenerator).ShaftWarmth);
CurrentGenerator.GetComponent(LightShaftGenerator).SoftShaftFactor = EditorGUILayout.Slider(GUIContent("Soft Shaft Factor:","This doest the same thing as Soft Particle Factor does in the particle/additive shader"), CurrentGenerator.GetComponent(LightShaftGenerator).SoftShaftFactor,0.01,3);


//Redirect color sampling to generator
 CurrentGenerator.GetComponent(LightShaftGenerator).SampleColor=GUILayout.Toggle(CurrentGenerator.GetComponent(LightShaftGenerator).SampleColor,GUIContent ("Sample Color","Disabled when mobile support is on!  Enable real time color sampling which gets blended with the above two values. Note: the object of which is sampled must have a mesh collider attached and the texture has to be set to -Advanced->Read/Write enabled)"));
//only show this part when color sampling is enabled
if(CurrentGenerator.GetComponent(LightShaftGenerator).SampleColor){
//Redirect static sample to generator
 CurrentGenerator.GetComponent(LightShaftGenerator).StaticSample=GUILayout.Toggle(CurrentGenerator.GetComponent(LightShaftGenerator).StaticSample,GUIContent ("Static Sample","Only sample color at the start of the first frame. This is ideal for colored static objects like windows, as it gives a good result with no performance cost"));
//Redirect sample distance to generator
CurrentGenerator.GetComponent(LightShaftGenerator).SampleDistance = EditorGUILayout.FloatField(GUIContent ("Sample Distance:","How far (behind) should the generator search for colors (on enabled object, read tooltip from Sample Color)"), CurrentGenerator.GetComponent(LightShaftGenerator).SampleDistance);
	}
	
	GUILayout.Label("");

	//enable or dissable shaft fading
	CurrentGenerator.GetComponent(LightShaftGenerator).enableFading=GUILayout.Toggle(CurrentGenerator.GetComponent(LightShaftGenerator).enableFading,GUIContent ("Shaft fading","If enabled shaft will fade when the player gets closer."));
	//only show if shaft fading is true
		if(CurrentGenerator.GetComponent(LightShaftGenerator).enableFading){
		GUILayout.BeginHorizontal();
			GUILayout.Label("Player position");
			CurrentGenerator.GetComponent(LightShaftGenerator).playerPosition = EditorGUILayout.ObjectField(CurrentGenerator.GetComponent(LightShaftGenerator).playerPosition , Transform, true);
			GUILayout.EndHorizontal();

						CurrentGenerator.GetComponent(LightShaftGenerator).fadeDistance = EditorGUILayout.Slider(GUIContent ("Fade distance:","How close can the player get before shafts start fading? Note that the shaft intensity counters this. You will need to set a higher Fade distance, when you have a higher intensity!"), CurrentGenerator.GetComponent(LightShaftGenerator).fadeDistance,0.01,140.0);
					}

	GUILayout.Label("");

	GUILayout.Label("Range/Collision settings:");
//Redirect layerMask to generator hitLayer
CurrentGenerator.GetComponent(LightShaftGenerator).hitLayer = EditorGUILayout.MaskField (GUIContent("Hit layers:","Enable or disable which layers the shafts can hit. By default all layers will be included"), CurrentGenerator.GetComponent(LightShaftGenerator).hitLayer, CurrentGenerator.GetComponent(LightShaftGenerator).layerList);



//Redirect static lightshaft to generator
CurrentGenerator.GetComponent(LightShaftGenerator).StaticLightShafts=GUILayout.Toggle(CurrentGenerator.GetComponent(LightShaftGenerator).StaticLightShafts,GUIContent ("Static LightShafts","Disables real-time collision: Recommended for systems that never have to collide with anything or to save memory on mobile devices"));
//Redirect source range to generator
 CurrentGenerator.GetComponent(LightShaftGenerator).SourceRange=GUILayout.Toggle(CurrentGenerator.GetComponent(LightShaftGenerator).SourceRange,GUIContent ("Source Range","If enabled the maximum cast distance will be set to the distance of the light source inside the slot below."));
//only show when source range is selected
if (CurrentGenerator.GetComponent(LightShaftGenerator).SourceRange){
//Redirect Light source to generator
CurrentGenerator.GetComponent(LightShaftGenerator).LightSource = EditorGUILayout.ObjectField(CurrentGenerator.GetComponent(LightShaftGenerator).LightSource , Light, true);
}
//only show when source range is false
if(!CurrentGenerator.GetComponent(LightShaftGenerator).SourceRange){
//Redirect max cast distance to generator
CurrentGenerator.GetComponent(LightShaftGenerator).MaxCastDistance = EditorGUILayout.FloatField(GUIContent ("Max Cast Distance:","How far can the maximum distance be for the shafts to be casted (for static cast this will be the first collision point with this range as its max))"), CurrentGenerator.GetComponent(LightShaftGenerator).MaxCastDistance);
}



GUILayout.Label("");
GUILayout.Label(GUIContent ("Shaft cast settings:","These settings control how the system will cast the shafts (amount, fall angle, spread width). Tooltips will tell you which of these settings will be overridden by above settings (mostly by certain cast types)."));

//Redirect all axis to generator
CurrentGenerator.GetComponent(LightShaftGenerator).showShaftRow = EditorGUILayout.Foldout(CurrentGenerator.GetComponent(LightShaftGenerator).showShaftRow, GUIContent ("Shaft rows", "Amount of shafts casted allong each axis. In general you want to leave Z on 1."));
if(CurrentGenerator.GetComponent(LightShaftGenerator).showShaftRow){
CurrentGenerator.GetComponent(LightShaftGenerator).ShaftRow = EditorGUILayout.Vector3Field(GUIContent ("","Amount of shafts casted allong each axis. In general you want to leave Z on 1."), CurrentGenerator.GetComponent(LightShaftGenerator).ShaftRow);

}


//Redirect ShaftWidth to generator
CurrentGenerator.GetComponent(LightShaftGenerator).showShaftWidth = EditorGUILayout.Foldout(CurrentGenerator.GetComponent(LightShaftGenerator).showShaftWidth, GUIContent ("Shaft width", "Set the shafts width of the Y and X axis. (z is controlled by the generator for its length)."));
if(CurrentGenerator.GetComponent(LightShaftGenerator).showShaftWidth){
CurrentGenerator.GetComponent(LightShaftGenerator).ShaftWidth = EditorGUILayout.Vector2Field(GUIContent ("","Set the shafts width of the Y and X axis. (z is controlled by the generator for its length"), CurrentGenerator.GetComponent(LightShaftGenerator).ShaftWidth);
}

//Redirect ShaftSpace to generator
CurrentGenerator.GetComponent(LightShaftGenerator).showShaftSpace = EditorGUILayout.Foldout(CurrentGenerator.GetComponent(LightShaftGenerator).showShaftSpace, GUIContent ("Shaft space", "Set per axis how much spacing should be between each of the shafts."));
if(CurrentGenerator.GetComponent(LightShaftGenerator).showShaftSpace){
CurrentGenerator.GetComponent(LightShaftGenerator).ShaftSpace = EditorGUILayout.Vector3Field(GUIContent ("","Distance between each shaft"), CurrentGenerator.GetComponent(LightShaftGenerator).ShaftSpace);
}

GUILayout.Label("");

GUILayout.Label("");

//Redirect all random toggle functions to the generator
 CurrentGenerator.GetComponent(LightShaftGenerator).RandomUpAngle=GUILayout.Toggle(CurrentGenerator.GetComponent(LightShaftGenerator).RandomUpAngle,GUIContent ("Random up angle","Casts randomly in all vertical angles, overrides Up angle."));
 CurrentGenerator.GetComponent(LightShaftGenerator).RandomWidthAngle=GUILayout.Toggle(CurrentGenerator.GetComponent(LightShaftGenerator).RandomWidthAngle,GUIContent ("Random width angle","Casts randomly in all horizontal angles, overrides Width angle."));


//Redirect all width and up angles/spread to the generator
//if random up is not selected
if(! CurrentGenerator.GetComponent(LightShaftGenerator).RandomUpAngle){
CurrentGenerator.GetComponent(LightShaftGenerator).UpAngle = EditorGUILayout.FloatField(GUIContent ("Up angle:","The vertical angle in which the shafts should be casted 0=straight"), CurrentGenerator.GetComponent(LightShaftGenerator).UpAngle);
CurrentGenerator.GetComponent(LightShaftGenerator).UpSpread = EditorGUILayout.FloatField(GUIContent ("Up spread:","This angle sets the spread falloff, 0=straight"), CurrentGenerator.GetComponent(LightShaftGenerator).UpSpread);
}
//if random up is not selected
if(!CurrentGenerator.GetComponent(LightShaftGenerator).RandomWidthAngle){
CurrentGenerator.GetComponent(LightShaftGenerator).WidthAngle = EditorGUILayout.FloatField(GUIContent ("Width angle:","The horizontal angle in which the shafts should be casted 0=straight"), CurrentGenerator.GetComponent(LightShaftGenerator).WidthAngle);
CurrentGenerator.GetComponent(LightShaftGenerator).WidthSpread = EditorGUILayout.FloatField(GUIContent ("Width spread:","This angle sets the spread falloff, 0=straight"), CurrentGenerator.GetComponent(LightShaftGenerator).WidthSpread);
}

GUILayout.Label("_________________________________________________________");	


//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//						
}

////////////////////////////////////////////////////////////
//-----Phase 3 setup-----//	
GUILayout.Label("");
CurrentGenerator.GetComponent(LightShaftGenerator).Phase3 = EditorGUILayout.Foldout(CurrentGenerator.GetComponent(LightShaftGenerator).Phase3, GUIContent ("Phase 3 setup", "Contains settings to reduce unwanted results and performance enhancment settings."));	
if(CurrentGenerator.GetComponent(LightShaftGenerator).Phase3){
GUILayout.Label("..........................................................................");
GUILayout.Label("Optimizations:");
//Redirect smooth toggle to generator
 CurrentGenerator.GetComponent(LightShaftGenerator).Smooth=GUILayout.Toggle(CurrentGenerator.GetComponent(LightShaftGenerator).Smooth,GUIContent ("Smooth","Never cast shafts in the same way as its neighbour. This prevents Moiré effect when watching from the side"));
//Redirect mobile support to generator
 CurrentGenerator.GetComponent(LightShaftGenerator).mobile=GUILayout.Toggle(CurrentGenerator.GetComponent(LightShaftGenerator).mobile,GUIContent ("Mobile/SharedMaterial","This affects all casters using the same material in the scene when enabled! Changes the way materials are created, this automatically disables real-time color sampling (if turned on, it will be turned off). Highly recommended for mobile builds or if none of your generators use different colors/real-time color sampling. All generators using the same material are affected. Use different materials per generator if you want to used sharedMaterial, but need different colors"));

 //re-create an array view in the inspector to manage cameras for sorting modes
CurrentGenerator.GetComponent(LightShaftGenerator).dropdownList= EditorGUILayout.Foldout(CurrentGenerator.GetComponent(LightShaftGenerator).dropdownList,GUIContent ("Transparency sorting modes","Cameras in this list will be set to the selected sorting mode. Main camera is selected standard"));
if(CurrentGenerator.GetComponent(LightShaftGenerator).dropdownList){

  	//Redirect sorting mode enum
 	CurrentGenerator.GetComponent(LightShaftGenerator).mode = EditorGUILayout.EnumPopup(GUIContent ("Sorting mode","Set one or more cameras their transparency sorting modes. Only change if you have sorting issues. WARNING! This option affects all transparent objects in the scene"),CurrentGenerator.GetComponent(LightShaftGenerator).mode);
	//Redirect sorting camera array
 	ArrayManager();

 

}
 
GUILayout.Label("..........................................................................");												

}
																						
//--------------------------------------------------------------------------------------------------------------------//
										
GUILayout.Label("");
if ( GUI.changed ){

Repaint();
 SceneView.RepaintAll();
}
		
	// Show default inspector property editor
	//DrawDefaultInspector ();


	} 
	
	
	
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	function LoadText(){
		previewTex=Resources.Load("UI/Preview_button");
		loadTex=Resources.Load("UI/Load_button");
		saveTex=Resources.Load("UI/Save_button");
		deleteTex=Resources.Load("UI/Delete_button");
 			}	
	
	
	
	function DeleteOldShafts(){
	 var CurrentGenerator = Selection.activeGameObject;
			 //first clear all old shafts before refreshing
          var tmpCount:int=CurrentGenerator.transform.childCount;
          if(tmpCount>=1){
          for (var child : Transform in CurrentGenerator.transform) {
		DestroyImmediate(child.gameObject, false);
		}
		}
	}
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	
		function ArrayManager(){
		//create an UI similar to that of an Array (to show and edit an actual array inside VolumetricAudio).

 				EditorGUILayout.BeginHorizontal();
						GUILayout.Label("Add/Remove Camera ");

					  	if(GUILayout.Button ("+")){
					  		CurrentGenerator.GetComponent(LightShaftGenerator).cam.Add(Camera.main);
					  			}
					  	
					  	if(GUILayout.Button ("-") && CurrentGenerator.GetComponent(LightShaftGenerator).cam.Count >=1){
					  		CurrentGenerator.GetComponent(LightShaftGenerator).cam.RemoveAt(CurrentGenerator.GetComponent(LightShaftGenerator).cam.Count-1);
					  			}
					  	if(GUILayout.Button ("clear") && CurrentGenerator.GetComponent(LightShaftGenerator).cam.Count >=1){
					 	 	CurrentGenerator.GetComponent(LightShaftGenerator).cam.Clear();
					  			}

					  		if(CurrentGenerator.GetComponent(LightShaftGenerator).cam.Count <= 0){
					  			CurrentGenerator.GetComponent(LightShaftGenerator).cam.Add(GameObject.FindWithTag("MainCamera").GetComponent.<Camera>());							  			 					  			
					  			}

					  			
				EditorGUILayout.EndHorizontal();
				
						GUILayout.Label("Size: "+	CurrentGenerator.GetComponent(LightShaftGenerator).cam.Count.ToString());
						
						
						//after adding or removing elements, initialise the array.
								initCamArray();
							
 
					}

		function initCamArray(){
			//for each tag in the array create a string field
 				if(CurrentGenerator.GetComponent(LightShaftGenerator).cam.Count >= 1){
 					for(var i:int ; i <= CurrentGenerator.GetComponent(LightShaftGenerator).cam.Count-1 ; i++){
 						CurrentGenerator.GetComponent(LightShaftGenerator).cam[i] = EditorGUILayout.ObjectField(GUIContent ("exclude "+ i,""), CurrentGenerator.GetComponent(LightShaftGenerator).cam[i],Camera);
							}
						}
					}
	
	
	
	
		// Custom GUILayout progress bar.
	function ProgressBar (value : float, label : String) {
		// Get a rect for the progress bar using the same margins as a textfield:
		var rect : Rect = GUILayoutUtility.GetRect (18, 18, "TextField");
		EditorGUI.ProgressBar (rect, value, label);
		EditorGUILayout.Space ();
	}
	
	