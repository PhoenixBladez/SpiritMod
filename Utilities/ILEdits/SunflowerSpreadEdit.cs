using log4net;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using SpiritMod.Tiles;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SpiritMod.Utilities.ILEdits
{
	class SunflowerSpreadEdit : ILEdit
	{
		public override void Load(Mod mod) => IL.Terraria.WorldGen.hardUpdateWorld += WorldGen_hardUpdateWorld;

		/// This IL edit is used to stop evil stones (ebonstone/crimstone) from spreading into areas protected by super sunflowers
		/// Evil grass is also handled in a separate detour (WorldGen_SpreadGrass)
		///
		/// This edit is simple. We just need to search for one instruction that loads NPC.downedPlantBoss (as all instructions afterwards have to do with evil spreading).
		/// Then we need to push the "i" parameter onto the stack (the x coordinate of the updated tile)
		/// 
		/// THESE ARE THE IL INSTRUCTIONS WE'LL HARASS AND WHAT IT'LL LOOK LIKE WHEN WE'RE DONE
		///   IL_XXXX: [WE INSERT AN INSTRUCTION HERE TO PUSH THE FIRST PARAMETER, i (THE X COORDINATE OF THE TILE)]
		///   IL_XXXX: [WE RUN OUR CODE HERE TO CHECK IF THE X COORDINATE IS IN A PROTECTED ZONE. WE PUSH TRUE ONTO THE STACK IF IT IS, OTHERWISE FALSE]
		///   IL_XXXX: [WE INSERT AN INSTRUCTION HERE TO CHECK THE BOOL WE PUSHED, AND SKIP THE NEXT INSTRUCTION IF IT IS FALSE]
		///   IL_XXXX: [WE INSERT AN INSTRUCTION HERE TO RETURN FROM THE METHOD. THIS IS ONLY REACHED IF THE ABOVE INSTRUCTION DOESN'T BRANCH]
		///   IL_050f: ldsfld bool [Terraria]Terraria.NPC::downedPlantBoss   <------ WE WILL JUMP TO THIS INSTRUCTION TO APPLY THE ABOVE EDITS
		private static void WorldGen_hardUpdateWorld(ILContext il)
		{
			// Get an ILCursor and a logger to report errors if we find any
			ILCursor cursor = new ILCursor(il);
			ILog logger = ModContent.GetInstance<SpiritMod>().Logger;

			// Try to jump to the specified instruction and stop if we can't find it
			// We use MoveType.AfterLabel because there are labels from previous instructions pointing to the location we're emitting to
			// and thus we want our emitted instructions to become the target for the labels, instead of the NPC.downedPlantBoss instruction
			if (!cursor.TryGotoNext(MoveType.AfterLabel, i => i.MatchLdsfld<NPC>("downedPlantBoss")))
			{
				logger.Error("Failed to patch WorldGen.hardUpdateWorld to add super sunflower functionality");
				return;
			}

			// We'll make use of this empty label shortly. It is supposed to target the NPC.downedPlantBoss instruction; but we initialize it later
			// This is to account for the fact that you can't spell "branch" without "bitch" and will save us from pain
			ILLabel label = cursor.DefineLabel();

			// Now that the cursor is behind the target instruction, we need to push the parameter "i" onto the stack
			cursor.Emit(OpCodes.Ldarg_0);

			// Now we run our own code that consumes "i" off the stack, checks if they're in a protected zone and returns the result of the check
			cursor.EmitDelegate<Func<int, bool>>(i => {
				foreach (Point16 point in MyWorld.superSunFlowerPositions)
					if (Math.Abs(point.X - i) < SuperSunFlower.Range * 2)
						return true;
				return false;
			});

			// We check the bool we pushed, and branch to the label we defined earlier (the one supposed to be looking at the NPC.downedPlantBoss instruction) if it is false
			// Doing so would skip over the return instruction we are about to emit
			cursor.Emit(OpCodes.Brfalse_S, label);

			// A return instruction that will get run if the tile is in a protected zone (meaning that the above branch didn't run)
			cursor.Emit(OpCodes.Ret);

			// We finish off by moving the cursor forwards again to the NPC.downedPlantBoss instruction and actually initializing the label
			// The reason we do this now is because emitting instructions can easily mess with labels and move them around
			// But since we only have 1 label to worry about we can just initialize it after everything has been emitted
			cursor.GotoNext(i => i.MatchLdsfld<NPC>("downedPlantBoss"));
			cursor.MarkLabel(label);
		}
	}
}
