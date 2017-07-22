#pragma strict


static function Percentage (goal:float, target:float,round:boolean):float {

	var newPercentage:float;
		newPercentage = goal/target*100;
		
//example
//goal(60) is what percent of target(400) 
//= 60 / 400 
//= 0.15 
//
//Converting decimal to a percentage: 
//0.15 * 100 = 15%


//rounded value
	if(round){
		newPercentage = Mathf.Round(newPercentage);
			}
		//unrounded value
		else{
			newPercentage=newPercentage;
				}

					return newPercentage;


}


static function DifferenceInt(value1:int, value2:int):int{

if(value1>=value2){
return (value1-value2);
}

if(value2>=value1){
return (value2-value1);
}


}



static function DifferenceFloat(value1:float, value2:float):float{

if(value1>=value2){
return (value1-value2);
}

if(value2>=value1){
return (value2-value1);
}


}

static function Map ( x: float, inMin: float, inMax: float, outMin: float, outMax:float):float{
//Debug.Log((x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin);
  return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
	}