using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Renderer rend = new Renderer();
    public float MaxSpeed = 6;
    public float Acceleration = 5;
    public float jumpSpeed = 5;
    public float jumpDuration;
	public bool isOnGround = false;
	public float rotationSpeed = 1;
	public bool rotateAroundObject = true;

    public bool enableDoubleJump = true;
    public bool wallHitJump = true;

    bool canDoubleJump = true;
    float jmpDuration;

    bool keyPressDown = false;
    bool canJumpVariable = false; 

	Quaternion defaultRot = new Quaternion();

    void Start()
    {

		defaultRot = transform.rotation;
        rend = GetComponent<Renderer>();

    }

	// Update is called once per frame
	void Update ()
    {
        
        float horizontal = Input.GetAxis("Horizontal");

        if(horizontal < -0.1f)
        {
            if(GetComponent<Rigidbody2D>().velocity.x > -this.MaxSpeed)
            {
                GetComponent<Rigidbody2D>().AddForce(new Vector2(-this.Acceleration, 0.0f));
            }
            //else
            //{
            //    GetComponent<Rigidbody2D>().velocity = new Vector2(-this.MaxSpeed, GetComponent<Rigidbody2D>().velocity.y);
            //}
        }
        else if(horizontal > 0.1f)
        {
            if (GetComponent<Rigidbody2D>().velocity.x < this.MaxSpeed)
            {
                GetComponent<Rigidbody2D>().AddForce(new Vector2(this.Acceleration, 0.0f));
            }
            //else
            //{
            //    GetComponent<Rigidbody2D>().velocity = new Vector2(this.MaxSpeed, GetComponent<Rigidbody2D>().velocity.y);
            //}
        }

        isOnGround = onGround();

        if (isOnGround)
        {
            canDoubleJump = true;
        }

        if (Input.GetKeyDown("w") || Input.GetKeyDown("joystick button 0"))
        {
            if (!keyPressDown)
            {
                keyPressDown = true;
                if (isOnGround || (canDoubleJump && enableDoubleJump || wallHitJump))
                {
                    bool wallHit = false;
                    int wallHitDirection = 0;

                    bool leftWallHit = onLeftWall();
                    bool rightWallHit = onRightWall();

                    if (horizontal != 0)
                    {
                        if (leftWallHit)
                        {
                            wallHit = true;
                            wallHitDirection = 1;
                        }
                        else if (rightWallHit)
                        {
                            wallHit = true;
                            wallHitDirection = -1;
                        }
                    }

                    if (!wallHit)
                    {
                        if (isOnGround || (canDoubleJump && enableDoubleJump))
                        {
                            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, this.jumpSpeed);
                            jumpDuration = 0.0f;
                            canDoubleJump = true;
                        }
                    }
                    else
                    {
                        GetComponent<Rigidbody2D>().velocity = new Vector2(this.jumpSpeed * wallHitDirection, this.jumpSpeed);

                        jmpDuration = 0.0f;
                        canJumpVariable = true;
                    }

                    if (!isOnGround && !wallHit)
                    {
                        canDoubleJump = false;
                    }
                }
            }
            else if (canJumpVariable)
            {
                jmpDuration += Time.deltaTime;

                if (jmpDuration < this.jumpDuration / 1000)
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, this.jumpSpeed);
                }
            }
        }
        else
        {
            keyPressDown = false;
            canJumpVariable = false;
        }

        if (Input.GetKeyDown("space"))
        {

        }
    }

    private bool onGround()
    {
        float checkLength = 0.5f;
        float colliderThreshold = 0.001f;

        Vector2 lineStart = new Vector2(this.transform.position.x, this.transform.position.y - rend.bounds.extents.y - colliderThreshold);

        Vector2 searchVector = new Vector2(this.transform.position.x, lineStart.y - checkLength);

		RaycastHit2D hit = Physics2D.Linecast(lineStart, searchVector);

        Debug.DrawLine(lineStart, searchVector,Color.green);
        //deal with rotating the character to match the object it is standing on

        if (rotateAroundObject) {
			if (hit) {
				if (hit.transform.gameObject.layer == 13) {
					if (transform.rotation.z != hit.transform.rotation.z) {
						if (hit.transform.rotation.z > -0.3f && hit.transform.rotation.z < 0.3f) {
							if (Mathf.Abs (transform.rotation.z - hit.transform.rotation.z) < 0.05f) {
								transform.rotation = hit.transform.rotation;
							} else {
								Quaternion temp = transform.rotation;
								temp.z = hit.transform.rotation.z / 4;
								transform.rotation = temp;
							}
						}
					}
				}
			} else {
				Quaternion temp = transform.rotation;
				temp.z += (0 - transform.rotation.z * Time.deltaTime) * rotationSpeed;
				transform.rotation = temp;
			}
		}

		return hit;
    }

    private bool onLeftWall()
    {

        float checkLength = 0.5f;
        float colliderThreshold = 0.01f;

        Vector2 lineStart = new Vector2(this.transform.position.x - rend.bounds.extents.x - colliderThreshold, this.transform.position.y);

        Vector2 searchVector = new Vector2(lineStart.x - checkLength, this.transform.position.y);

        RaycastHit2D hit = Physics2D.Linecast(lineStart, searchVector);
        Debug.DrawLine(lineStart, searchVector, Color.red);

        return hit;
    }

    private bool onRightWall()
    {
       
        float checkLength = 0.5f;
        float colliderThreshold = 0.01f;

        Vector2 lineStart = new Vector2(this.transform.position.x + rend.bounds.extents.x + colliderThreshold, this.transform.position.y);
       

        Vector2 searchVector = new Vector2(lineStart.x + checkLength, this.transform.position.y);

        RaycastHit2D hit = Physics2D.Linecast(lineStart, searchVector);
        Debug.DrawLine(lineStart, searchVector,Color.red);
        return hit;

    }

}
