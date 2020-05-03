using System;
using Assets.KnifeHit.WheelModule;
using EventSystem;
using UnityEngine;

namespace Assets.KnifeHit.Knives
{
	[RequireComponent(typeof(FixedJoint2D))]
	[RequireComponent(typeof(Rigidbody2D))]
	public class KnifeBehaviour : MonoBehaviour, ICanPierce,IThrowable
	{
		[SerializeField] 
		private Settings _settings;
		
		private bool _isAttached;
		private bool _hasTouchedOtherKnife;
		private Rigidbody2D RigidBody2D { get; set; }
		private FixedJoint2D FixedJoint2D { get; set; }
		
		void Awake()
		{
			RigidBody2D = GetComponent<Rigidbody2D>();
			RigidBody2D.mass = _settings.mass;

			FixedJoint2D = GetComponent<FixedJoint2D>();
			FixedJoint2D.autoConfigureConnectedAnchor = true;
		}

		private void OnEnable()
		{
			FixedJoint2D.enabled = false;
			RigidBody2D.simulated = false;
		}

		public void Throw()
		{
			RigidBody2D.simulated = true;
			RigidBody2D.AddForce(Vector3.up * _settings.throwForce);
		}

		void OnCollisionEnter2D(Collision2D collisionData)
		{
			if (collisionData.collider.CompareTag($"Knife") || _hasTouchedOtherKnife)
			{
				_hasTouchedOtherKnife = true;
				return;
			}
			var objectThatCanBePierced = collisionData.collider.GetComponentInParent<ICanBePierced>();
			if (objectThatCanBePierced == null)
				return;
			if (!(Vector3.Angle(collisionData.contacts[0].normal, -transform.up) < _settings.thresholdAngle)) return;
			
			objectThatCanBePierced?.OnPierced(this);
			_isAttached = true;
		}
		void OnCollisionExit2D(Collision2D collisionData)
		{
			if (collisionData.collider.CompareTag($"Knife")  && !_isAttached)
			{
				EventManager.Raise(CustomEventType.OnLevelFailed);
				return;
			}
			var objectThatCanBePierced = collisionData.collider.GetComponentInParent<ICanBePierced>();
			if (objectThatCanBePierced == null && !_isAttached)
			{
				Debug.Log("Level Failed!");
				EventManager.Raise(CustomEventType.OnLevelFailed);
			}
		}

		public void Pierce( ICanBePierced canPierceObject)
		{
			FixedJoint2D.connectedBody = canPierceObject.WheelRigidbody;
			FixedJoint2D.enabled = true;
		}

		[Serializable]
		public class Settings
		{
			public float throwForce;
			public float mass;
			public float thresholdAngle;
		}
	}
}

