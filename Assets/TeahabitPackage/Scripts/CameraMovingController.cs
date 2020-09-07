using UnityEngine;
using System.Collections;

namespace TeaSoft
{

	public class CameraMovingController : MonoBehaviour
	{

	    public float horizantalLookSpeed = 15f;
	    public float verticalLookSpeed = 10f;
	    Quaternion originalRotation;
	    float distanceHorizantal = 0f;
	    float distanceVetical = 0f;
	    public float movingSpeed = 10f;
	    float scrollWheelSpeedBouns = 100;
	    float accelerateBouns = 1f;


	    // Use this for initialization
	    void Start()
	    {
	        originalRotation = transform.localRotation;
	    }

	    // Update is called once per frame
	    void Update()
	    {

	        if (Input.GetMouseButton(1))
	        {
	            distanceHorizantal += Input.GetAxis("Mouse X") * horizantalLookSpeed;
	            distanceVetical += Input.GetAxis("Mouse Y") * verticalLookSpeed;

	            distanceHorizantal = ClampAngle(distanceHorizantal, -360, 360);
	            distanceVetical = ClampAngle(distanceVetical, -360, 360);

	            Quaternion xQuaternion = Quaternion.AngleAxis(distanceHorizantal, Vector3.up);
	            Quaternion yQuaternion = Quaternion.AngleAxis(distanceVetical, Vector3.left);

	            transform.localRotation = originalRotation * xQuaternion * yQuaternion;
	        }
	        if (Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")) > 0)
	        {
	            this.transform.Translate(this.transform.InverseTransformDirection(this.transform.forward) * Input.GetAxis("Mouse ScrollWheel") * scrollWheelSpeedBouns * Time.deltaTime * movingSpeed);
	        }
	        if (Input.GetKey(KeyCode.W))
	        {
	            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
	            {
	                accelerateBouns = 3f;
	            }
	            else
	            {
	                accelerateBouns = 1f;
	            }
	            this.transform.Translate(this.transform.InverseTransformDirection(this.transform.forward) * Time.deltaTime * movingSpeed * accelerateBouns);
	        }
	        if (Input.GetKey(KeyCode.S))
	        {
	            this.transform.Translate(this.transform.InverseTransformDirection(this.transform.forward) * Time.deltaTime * movingSpeed * (-1f));
	        }
	        if (Input.GetKey(KeyCode.A))
	        {
	            this.transform.Translate(this.transform.InverseTransformDirection(this.transform.right) * Time.deltaTime * movingSpeed * (-1f));
	        }
	        if (Input.GetKey(KeyCode.D))
	        {
	            this.transform.Translate(this.transform.InverseTransformDirection(this.transform.right) * Time.deltaTime * movingSpeed);
	        }
	    }

	    public static float ClampAngle(float angle, float min, float max)
	    {

	        if (angle < -360F)

	            angle += 360F;

	        if (angle > 360F)

	            angle -= 360F;

	        return Mathf.Clamp(angle, min, max);
	    }
	}
}