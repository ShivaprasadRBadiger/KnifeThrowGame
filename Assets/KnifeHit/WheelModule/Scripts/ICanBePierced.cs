using Assets.KnifeHit.Knives;
using UnityEngine;

namespace Assets.KnifeHit.WheelModule
{
	public interface ICanBePierced
	{
		void OnPierced(ICanPierce objectThatCanPierce);
		Rigidbody2D WheelRigidbody { get; }
	}
}