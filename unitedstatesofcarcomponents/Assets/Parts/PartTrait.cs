using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PartTrait
{
    NO_TRAIT, // Does nothing
    RED, // All blue parts are disabled, always effective
    BLUE, // Car goes faster, but part does nothing if you have red parts.
    ELECTRIC, // If you have at least three electric parts, none of them work
    CONFUSED // Steering is inverted
}

public static class TraitExtensions {
    public static string Description(this PartTrait trait) {

        switch (trait) {
            case PartTrait.RED: return "Red: Disables blue parts";
            case PartTrait.BLUE: return "Blue: Part is more durable, but won't work if the car has Red parts.";
            case PartTrait.ELECTRIC: return "Electric: Works just fine, so long as you don't have too many.";
            case PartTrait.CONFUSED: return "Confused: Gives people Lego Racers flashbacks";
            default: return $"{trait}: NO_DESCRIPTION_FOR_TRAIT: ";
        }
    }

    /**
     * Returns whether or not a trait is enabled, given that it's attached to a part with certain other traits, as well as some set of other parts.
     */
    public static bool IsEnabled(this PartTrait trait, List<PartTrait> allOwnPartTraits, List<CarPart> otherParts) {
        switch (trait) {
            case PartTrait.RED:
                return true; // Does nothing on its own, but might as well show it as enabled if we're providing visual feedback for this stuff (I assume we aren't, but could be neat for debugging?).
            case PartTrait.BLUE:
                foreach (CarPart part in otherParts) {
                    if (part.traits.Contains(PartTrait.RED)) return true;
                }

                return false;
            case PartTrait.ELECTRIC:
                int electricCounter = 1;

                foreach (CarPart part in otherParts) {
                    if (part.traits.Contains(PartTrait.ELECTRIC)) {
                        electricCounter++;
                        if (electricCounter >= 3) return true;
                    }
                }

                return false;
            case PartTrait.CONFUSED:
                return true;
            default: return false;
        }
    }

    /**
     * Returns whether or not a trait inverts the car's steering (if the trait is enabled)
     */
    public static bool InvertsSteering(this PartTrait trait) {
        switch (trait) {
            case PartTrait.CONFUSED: return true;
            default: return false;
        }
    }

    /**
     * Returns whether or not a trait disables the part it's on (if the trait is enabled)
     */
    public static bool DisablesOwnPart(this PartTrait trait) {
        switch (trait) {
            case PartTrait.BLUE:
            case PartTrait.ELECTRIC:
                return true;
            default: return false;
        }
    }

    public static int PartHealthImpact(this PartTrait trait) {
        switch (trait) {
            case PartTrait.BLUE: return 20;
            default: return 0;
        }
    }

    public static float MaxSpeedMult(this PartTrait trait) {
        switch (trait) {
            case PartTrait.BLUE: return 1.2F;
            default: return 1;
        }
    }
}
