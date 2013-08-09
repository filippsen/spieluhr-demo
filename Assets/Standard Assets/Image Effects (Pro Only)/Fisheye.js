

public var strengthX : float = 0.05;
public var strengthY : float = 0.05;


@script ExecuteInEditMode
@script AddComponentMenu ("Image Effects/Fisheye")

class Fisheye extends PostEffectsBase {
	
	public var fishEyeShader : Shader = null;
	private var _fisheyeMaterial : Material = null;	
	
	function CreateMaterials () {
		if (!_fisheyeMaterial) {
			if(!CheckShader(fishEyeShader)) {
				enabled = false;
				return;	
			}
			_fisheyeMaterial = new Material (fishEyeShader);	
			_fisheyeMaterial.hideFlags = HideFlags.HideAndDontSave;
		}
	}
	
    var timer:float;
    var stepX:float;
    var stepY:float;
	function Start () {
		CreateMaterials ();

        timer = Time.time;
        stepX = 0.005f;
        stepY = 0.005f;
	}
	
    function Update()
    {
        if(Time.time - timer > 24 * 7 + 5)
        {
            //print("update level 5");
            //print("lerping strength");
            strengthX += 0.0001;
            if(strengthX > 1.0f || strengthX < -1.0f)
                stepX*= -1.0;   
        }
          
        else if(Time.time - timer > 24 * 6 + 5)
        {
         //print("update level 4");
            //print("lerping strength");
            strengthX = Mathf.Lerp(strengthX, 0.2110436, Time.deltaTime);
            strengthY = Mathf.Lerp(strengthY, 0.4699998, Time.deltaTime);
        }
        else if(Time.time - timer > 24 * 5 + 5)
        {
         //print("update level 3");
            strengthX += stepX*0.5f;//+Random.Range(0, 0.1);
            strengthY += stepY*2.5f;//+Random.Range(0, 0.1);
            if(strengthY > 1.0f || strengthY < -1.0f)
                stepY*= -1.0;        
            if(strengthX > 1.0f || strengthX < -1.0f)
                stepX*= -1.0;     
        }
        else if(Time.time - timer > 24 * 4 + 5)
        {
         //print("update level 2");
            strengthY += stepY;
            if(strengthY > 1.0f || strengthY < -1.0f)
                stepY*= -1.0;            
        }
        else  if(Time.time - timer > 24 * 3 + 5)
        {
        //print("update level 1");
            strengthX += stepX;
            if(strengthX > 1.0f || strengthX < -1.0f)
                stepX*= -1.0;            
        }
        

    }
	function OnRenderImage (source : RenderTexture, destination : RenderTexture)
	{		
		CreateMaterials ();
		
		var ar : float = (source.width * 1.0) / (source.height * 1.0);
		
		_fisheyeMaterial.SetVector ("intensity", Vector4 (strengthX * ar, strengthY * ar, strengthX * ar, strengthY * ar));
		Graphics.Blit (source, destination, _fisheyeMaterial); 	
	}
}