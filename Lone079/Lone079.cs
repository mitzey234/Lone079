using HarmonyLib;
using Exiled.API.Features;

namespace Lone079
{
	public class Lone079 : Plugin<Config>
	{
		public static Lone079 instance;
		private EventHandlers ev;

		private Harmony hInstance;

		private bool state = false;

		public override void OnEnabled()
		{
			if (state) return;

			if (!Config.IsEnabled) return;

			instance = this;

			hInstance = new Harmony("cyanox.lone079");
			hInstance.PatchAll();

			ev = new EventHandlers();

			Exiled.Events.Handlers.Server.RoundStarted += ev.OnRoundStart;
			Exiled.Events.Handlers.Player.Died += ev.OnPlayerDied;
			Exiled.Events.Handlers.Player.Left += ev.OnPlayerLeave;
			Exiled.Events.Handlers.Scp106.Containing += ev.OnScp106Contain;
			Exiled.Events.Handlers.Warhead.Detonated += ev.OnDetonated;

			state = true;
			base.OnEnabled();
			Exiled.Events.Handlers.Cassie.SendingCassieMessage += ev.OnCassie;
		}

		public override void OnDisabled()
		{
			if (!state) return;

			Exiled.Events.Handlers.Server.RoundStarted -= ev.OnRoundStart;
			Exiled.Events.Handlers.Player.Died -= ev.OnPlayerDied;
			Exiled.Events.Handlers.Player.Left -= ev.OnPlayerLeave;
			Exiled.Events.Handlers.Scp106.Containing -= ev.OnScp106Contain;
			Exiled.Events.Handlers.Warhead.Detonated -= ev.OnDetonated;
			Exiled.Events.Handlers.Cassie.SendingCassieMessage -= ev.OnCassie;

			hInstance.UnpatchAll(hInstance.Id);

			ev = null;

			state = false;
			base.OnDisabled();
		}

		public override string Name => "Lone079";
	}
}
