using Assets.KnifeHit.GameSettings;
using Assets.KnifeHit.Knives;
using EventSystem;
using UnityEngine;

namespace Assets.KnifeHit.WheelModule
{
	[RequireComponent(typeof(WheelJoint2D))]
	public class WheelBehaviour : MonoBehaviour, ICanBePierced
	{
		[SerializeField] private GameWideSettings gameWideSettings;
		private Settings _settings;
		[Range(0, 1)]
		[SerializeField]
		private float _targetLevelProgression;

		WheelJoint2D _wheelJoint;

		[SerializeField] private Rigidbody2D wheelRigidbody;
		public Rigidbody2D WheelRigidbody => wheelRigidbody;


		private JointMotor2D _wheelMotor;
		private int _currentKnivesHit=0;
		private int _totalKnivesAvailable;
		private float _levelProgression;
		private float _levelProgressionVelocity;

		private void Awake()
		{
			_settings = gameWideSettings.WheelBehaviourSettings;
			_totalKnivesAvailable = gameWideSettings.KnifeThrowSettings.availableKnives;
			
			_wheelJoint = GetComponent<WheelJoint2D>();
			_wheelMotor = new JointMotor2D();
		}

		public void FixedUpdate()
		{
			_levelProgression = Mathf.SmoothDamp(_levelProgression, _targetLevelProgression,
				ref _levelProgressionVelocity, _settings.smoothTimeProgressionTransition);
			OnLevelProgressChanged();
		}

		void OnEnable()
		{
			_wheelMotor.maxMotorTorque = _settings.maxTorque;
			_wheelMotor.motorSpeed = _settings.angularSpeedOverTime.Evaluate(_levelProgression);
			_wheelJoint.motor = _wheelMotor;
		}
		void OnLevelProgressChanged()
		{
			_wheelMotor.motorSpeed = _settings.angularSpeedOverTime.Evaluate(_levelProgression);
			_wheelJoint.motor = _wheelMotor;
		}
		public void OnPierced(ICanPierce objectThatCanPierce)
		{
			objectThatCanPierce.Pierce( this);
			_currentKnivesHit++;
			_targetLevelProgression = _currentKnivesHit / (float)_totalKnivesAvailable;
			if (_currentKnivesHit == _totalKnivesAvailable)
			{
				EventManager.Raise(CustomEventType.OnLevelCompleted);
			}
		}

		[System.Serializable]
		public class Settings
		{
			public AnimationCurve angularSpeedOverTime;
			public float maxTorque;
			public float smoothTimeProgressionTransition;
		}
	}
}
