using Microsoft.Xna.Framework;
using SpiritMod.Mechanics.BackgroundSystem.BGItem;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.StarjinxEvent.Enemies.Archon
{
	class ArchonBG : BaseBGItem
	{
		private readonly NPC Parent;

		private Archon ArchonParent => Parent.ModNPC as Archon;

		public ArchonBG(Vector2 pos, NPC parent) : base(pos, 0f, new Point(0, 0))
		{
			tex = ModContent.Request<Texture2D>("SpiritMod/NPCs/StarjinxEvent/Enemies/Archon/Archon");
			source = new Rectangle(0, 0, tex.Width, tex.Height);
			scale = 1f;

			Parent = parent;
		}

		internal override void Behaviour()
		{
			Center = Parent.Center;

			scale = .5f;
			BaseParallax(.75f);
		}

		internal override void Draw(Vector2 off)
		{
			if (ArchonParent.inFG)
				return;

			drawColor = Color.Lerp(Main.bgColor, Color.White, 0.5f);
			base.Draw(GetParallax());
		}
	}
}
