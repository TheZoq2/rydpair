using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Manufacturers { NII_SAN, VOLVIMUS, SM }

public class Bolagsverket
{
	public static string GetName(Manufacturers manu)
	{
		switch (manu)
		{
			case Manufacturers.NII_SAN:
				return "Nii-san";
			case Manufacturers.VOLVIMUS:
				return "Volvimus";
			case Manufacturers.SM:
				return "SM";
			default:
				return "Unknown";
		}
	}
}