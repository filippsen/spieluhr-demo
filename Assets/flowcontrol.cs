using UnityEngine;

public class flowcontrol : MonoBehaviour 
{
    float bpminterval = 0.42f;
    float lastTime;
    float timePassedSinceIntro;
    const float introLength = 2.8502f;
    bool isLoopPlaying = false;
    float loopTimer;

    AudioSource audioLoop;

    public Vector4 f0 = new Vector4(243.9028f, 4.383772f, 27.37137f, 300f);
    public Vector4 f1 = new Vector4(305.5934f, 30.0f, 2.461121f, 6.0f);
    public bool frequencyChange = true;
    public bool setupChange = true;
    public bool gainChange = true;
    public bool stepControlled = true; //else: random controlled

    Vector4 innerColorDestination;
    Vector4 innerColor;
    Vector4 bgColorDestination;
    Vector4 bgColor;

    Color alphaColor = Color.black;

    public bool controlsOthers = false;

    void Awake()
    {
        innerColorDestination = GameObject.Find("inner").renderer.material.GetColor("diffuse_color");
        bgColorDestination = GameObject.Find("background").renderer.material.GetColor("diffuse_color");
        GameObject.Find("fakealpha").renderer.material.SetColor("_Color", Color.black);

        currentMix = this.renderer.material.GetFloat("ns_mix");
        currentPhaseMixX = this.renderer.material.GetFloat("shift_x");
        currentPhaseMixY = this.renderer.material.GetFloat("shift_y");

        blurObject = GameObject.Find("Main Camera").GetComponent<BlurEffect>();
        blurState = blurObject.enabled;
    }

	void Start () 
    {
        lastTime = Time.time;
        GameObject.Find("apintro").audio.Play();
        audioLoop = GameObject.Find("ap").audio;

        timePassedSinceIntro = Time.time;
	}

    const float range = 25;
    float step = 0.1f;
    float stepTimer;
    const float stepTimerMax = 10.0f;

    public bool isBackgroundSpecialSupreme = false;
    float currentMix;
    float mixStep = 0.1f;
    float currentPhaseMixX;
    float currentPhaseMixY;
    float phaseMixStepX = 0.1f;
    float phaseMixStepY = 0.1f;

    int bpmCounter = 0;
    int bpmPatternMode = 0;


    bool isVortexActivated = false;
    float vortexStep = 0.01f;

    bool isTwirlActivated = false;
    bool doTwirlOneTimer = true;
    float twirlStep = 0.01f;

    bool isBlurActivated = false;
    BlurEffect blurObject;
    bool blurState;

	void Update () 
    {
        // uncomment for tinting
        //innerColor = Vector4.Lerp(Color.black, innerColorDestination, Time.deltaTime * 0.05f);
        //GameObject.Find("inner").renderer.material.SetColor("diffuse_color", innerColor);
        //bgColor = Vector4.Lerp(Color.black, bgColorDestination, Time.deltaTime * 0.05f);
        //GameObject.Find("background").renderer.material.SetColor("diffuse_color", bgColor);

        alphaColor.a -= 0.0015f;
        GameObject.Find("fakealpha").renderer.material.SetColor("_Color", alphaColor);

        if (Time.time - timePassedSinceIntro > introLength
            && !isLoopPlaying)
        {
            audioLoop.Play();
            isLoopPlaying = true;
            loopTimer = Time.time;
        }

        if(isLoopPlaying)
        {
            if (controlsOthers)
            {
                if (bpmCounter > 55)
                {
                    isVortexActivated = true;
                }


                if (Time.time - loopTimer > 24*2)
                {
                    if (!isTwirlActivated)
                    {
                        isBlurActivated = true;

                        //UGLY:
                        GameObject.Find("Main Camera").GetComponent<TwirlEffect>().angle = Mathf.Lerp(GameObject.Find("Main Camera").GetComponent<TwirlEffect>().angle, 40, Time.deltaTime*2);
                        if (GameObject.Find("Main Camera").GetComponent<TwirlEffect>().angle > 30)
                        {
                            isTwirlActivated = true;

                            isBlurActivated = false;
                            blurState = false;
                            blurObject.enabled = blurState;
                        }
                    }
                }

                //if (bpmCounter % 49 == 0)
                if (Time.time - loopTimer > 24.2 * 3)
                {
                    //print("twirl deactivated");
                    isTwirlActivated = false;
                }

                //if (bpmCounter > 55 * 1
                //    && bpmCounter < 55 * 1 + 10)
                if (Time.time - loopTimer > 24)
                {
                    if (!isTwirlActivated)
                    {
                        isBlurActivated = true;

                        GameObject.Find("Main Camera").GetComponent<TwirlEffect>().angle = Mathf.Lerp(GameObject.Find("Main Camera").GetComponent<TwirlEffect>().angle, 40, Time.deltaTime * 2);
                        if (GameObject.Find("Main Camera").GetComponent<TwirlEffect>().angle > 30)
                        {
                            //    doTwirlOneTimer = false;
                            isTwirlActivated = true;

                            isBlurActivated = false;
                            blurState = false;
                            blurObject.enabled = blurState;
                        }
                    }
                }                

                if (isTwirlActivated)
                {
                    //UGLY:
                    GameObject.Find("Main Camera").GetComponent<TwirlEffect>().angle += twirlStep;

                    if (GameObject.Find("Main Camera").GetComponent<TwirlEffect>().angle > 30
                        || GameObject.Find("Main Camera").GetComponent<TwirlEffect>().angle < -30)
                        twirlStep *= -1;
                }
            }

            if (Time.time - lastTime > bpminterval)
            {
                bpmCounter++;

                lastTime = Time.time;

                if (controlsOthers)
                {
                    if (isBlurActivated)
                    {
                        //print("toggle blur");
                        blurState = !blurState;
                        blurObject.enabled = blurState;
                    }
                }
                if (isBackgroundSpecialSupreme)
                {
                    if (Time.time - loopTimer < 10)
                    {
                        if ((Time.time - loopTimer) % 0.3f >= 0 &&
                            (Time.time - loopTimer) % 0.4f <= 1)
                        {
                            //print("bpmCounter % 4 = " + bpmCounter % 4);
                            if (bpmPatternMode == 0)
                                bpmPatternMode = 1;
                            else if (bpmPatternMode == 1)
                                bpmPatternMode = 0;

                            else if (bpmPatternMode == 2)
                            {
                                bpmPatternMode = 0;

                                currentMix = 0;
                                this.renderer.material.SetFloat("ns_mix", currentMix);
                            }
                        }

                        
                        if (bpmCounter % 8 == 0)
                        {
                            //print("flash mode");
                            bpmPatternMode = 2;
                        }

                    }
                    else 
                    {
                        if (bpmCounter % 4 == 0)
                        {
                            //print("bpmCounter % 4 = " + bpmCounter % 4);
                            if (bpmPatternMode == 0)
                                bpmPatternMode = 1;
                            else if (bpmPatternMode == 1)
                                bpmPatternMode = 0;

                            else if (bpmPatternMode == 2)
                            {
                                bpmPatternMode = 0;

                                currentMix = 0;
                                this.renderer.material.SetFloat("ns_mix", currentMix);
                            }
                        }

                        if (bpmCounter % 16 == 0)
                        {
                            //print("flash mode");
                            bpmPatternMode = 2;
                        }

                    }
                    if (bpmCounter % 32 == 0)
                    {
                        //print("bpm 32");
                        phaseMixStepX *= -1.0f;
                        phaseMixStepY *= -1.0f;
                    }

                    if (bpmPatternMode == 0)
                    {
                        //print("pattern 0");
                        currentPhaseMixX += phaseMixStepX;
                        this.renderer.material.SetFloat("shift_x", currentPhaseMixX);
                    }
                    else if (bpmPatternMode == 1)
                    {
                        //print("pattern 1");
                        currentPhaseMixY += phaseMixStepY;
                        this.renderer.material.SetFloat("shift_y", currentPhaseMixY);
                    }
                    else if (bpmPatternMode == 2)
                    {
                        if(currentMix == 1)
                            currentMix = 0;
                        else if (currentMix == 0)
                            currentMix = 20;
                        this.renderer.material.SetFloat("ns_mix", currentMix);
                    }
                }
                else
                {
                    this.renderer.material.SetFloat("ns_mix", Random.Range(10, 50));
                    this.renderer.material.SetFloat("shift_x", Random.Range(10, 50));
                    this.renderer.material.SetFloat("shift_y", Random.Range(10, 50));
                }

                if (stepControlled)
                {
                    if (Time.time - stepTimer > stepTimerMax)
                    {
                        stepTimer = Time.time;
                        step *= -1;
                    }
                    if (frequencyChange)
                    {
                        f0.x += step;
                        f0.y += step;
                        //f1.x += Random.Range(range, range);
                        //f1.y += Random.Range(-range, range);
                    }
                    if (setupChange)
                    {
                        f0.z += step;
                        //f1.z += Random.Range(-range, range);
                    }

                    if (gainChange)
                    {
                        f0.w += step;
                        //f1.w += Random.Range(-range, range);
                    }
                }
                else
                {
                    //random
                    if (frequencyChange)
                    {
                        f0.x += Random.Range(-range, range);
                        f0.y += Random.Range(-range, range);
                        //f1.x += Random.Range(range, range);
                        //f1.y += Random.Range(-range, range);
                    }
                    if (setupChange)
                    {
                        f0.z += Random.Range(-range, range);
                        //f1.z += Random.Range(-range, range);
                    }

                    if (gainChange)
                    {
                        f0.w += Random.Range(-range, range);
                        //f1.w += Random.Range(-range, range);
                    }
                }

                this.renderer.material.SetVector("freq_0", f0);
                this.renderer.material.SetVector("freq_1", f1);
            }
        }
	}
}
