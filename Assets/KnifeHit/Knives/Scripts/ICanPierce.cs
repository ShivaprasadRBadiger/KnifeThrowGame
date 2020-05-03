using Assets.KnifeHit.WheelModule;

namespace Assets.KnifeHit.Knives
{
	public interface ICanPierce
	{
		void Pierce(ICanBePierced canPierceObject);
	}
}