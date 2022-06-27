using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Sets.BloodcourtSet;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace SpiritMod.NPCs.Critters
{
	public class DreamstrideWisp : SpiritNPC, IDrawAdditive
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dreamstride Wisp");
			Main.npcFrameCount[NPC.type] = 5;
			Main.npcCatchable[NPC.type] = true;
		}

		public override void SetDefaults()
		{
			NPC.width = 32;
			NPC.height = 32;
			NPC.damage = 0;
			NPC.defense = 0;
			NPC.lifeMax = 5;
			NPC.dontCountMe = true;
			NPC.catchItem = (short)ModContent.ItemType<DreamstrideEssence>();
			NPC.knockBackResist = .45f;
			NPC.aiStyle = 64;
			NPC.npcSlots = 0;
			NPC.noGravity = true;
			NPC.alpha = 255;
			NPC.scale = Main.rand.NextFloat(1.1f, 1.3f);

			AIType = NPCID.Firefly;

			if (!Main.dedServ)
			{
				NPC.HitSound = SoundID.NPCHit36.WithVolume(0.5f);
				NPC.DeathSound = SoundID.NPCDeath39.WithVolume(0.5f);
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY), NPC.frame, Color.White * NPC.Opacity, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
			return false;
		}

		public void AdditiveCall(SpriteBatch spriteBatch)
		{
			Texture2D circleGradient = Mod.Assets.Request<Texture2D>("Effects/Masks/CircleGradient").Value;
			spriteBatch.Draw(circleGradient, NPC.Center - Main.screenPosition, null, Color.Red * 0.8f * NPC.Opacity, 0, circleGradient.Size() / 2, new Vector2(0.33f, 0.45f) * NPC.scale, SpriteEffects.None, 0);
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (!Main.bloodMoon || !MyWorld.downedOccultist) 
				return 0f;
			return SpawnCondition.OverworldNight.Chance * 0.12f;
		}

		public override void AI()
		{
			ignorePlatforms = true;
			UpdateYFrame(7, 0, 4);
			NPC.rotation = NPC.velocity.X * 0.1f;
			Lighting.AddLight(NPC.Center, NPC.Opacity * Color.Red.ToVector3());

			if (!Main.dayTime)
				NPC.alpha = (int)MathHelper.Max(NPC.alpha - 3, 80);
			else
			{
				NPC.alpha += 10;
				if(NPC.alpha >= 255)
				{
					NPC.active = false;
					NPC.netUpdate = true;
				}
			}
		}

		public override void OnKill() => Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ModContent.ItemType<DreamstrideEssence>());
	}
}
