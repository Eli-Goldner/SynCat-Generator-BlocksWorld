﻿using System;
using System.Text;
using System.Collections.Generic;

namespace SynCatGenerator
{
    public class SynCat
    {
        public SynCat()
        {
        }
        public List<string> GetNPs()
        {
	    string[] sg_nouns = {"box", "block", "mug", "cup", "knife", "plate", "one"};
            string[] pl_nouns = {"blocks", "boxes", "mugs","cups","knives", "plates", "ones"};
            string[] sg_determiners = { "this", "that", "the" };
            string[] pl_determiners = { "these", "those", "the"};
            string[] adjectives = { "yellow", "red", "blue", "purple", "green", "orange", "white",
		"gray", "black", "pink", "brown" };
            string[] prepositions = { "on the left of", "on the right of", "to the left of",
		"to the right of", "left of", "right of", "above", "below", "behind", "in", "on",
                "beside", "before", "around",  "on top of", "in front of", "in back of", "on the front of",
		"on the back of"};
            List<string> sg_det_no_adj = new List<string>();
	    List<string> pl_det_no_adj = new List<string>();
            List<string> sg_det_adj = new List<string>();
	    List<string> pl_det_adj = new List<string>();
            List<string> sg_adj_no_det = new List<string>();
	    List<string> pl_adj_no_det = new List<string>();
	    List<string> NPs = new List<string>();

	    foreach (string adj in adjectives)
	    {
		foreach (string nn in sg_nouns)
		{
		    StringBuilder builder = new StringBuilder();
		    builder.Append(adj).Append(" ").Append(nn);
		    sg_adj_no_det.Add(builder.ToString().Trim());
		}
		foreach (string nn in pl_nouns)
		{
		    StringBuilder builder = new StringBuilder();
		    builder.Append(adj).Append(" ").Append(nn);
		    pl_adj_no_det.Add(builder.ToString().Trim());
		}
	    }
	    
	    foreach (string sg_det in sg_determiners)
	    {
		foreach (string nn in sg_nouns)
		{
		    StringBuilder builder = new StringBuilder();
		    builder.Append(sg_det).Append(" ").Append(nn);
		    sg_det_no_adj.Add(builder.ToString().Trim());
		}
		foreach (string adj_nn in sg_adj_no_det)
		{
		    StringBuilder builder = new StringBuilder();
		    builder.Append(sg_det).Append(" ").Append(adj_nn);
		    sg_det_adj.Add(builder.ToString().Trim());
		}
	    }

	    foreach (string pl_det in pl_determiners)
	    {
		foreach (string nn in pl_nouns)
		{
		    StringBuilder builder = new StringBuilder();
		    builder.Append(pl_det).Append(" ").Append(nn);
		    pl_det_no_adj.Add(builder.ToString().Trim());
		}
		foreach (string adj_nn in pl_adj_no_det)
		{
		    StringBuilder builder = new StringBuilder();
		    builder.Append(pl_det).Append(" ").Append(adj_nn);
		    pl_det_adj.Add(builder.ToString().Trim());
		}
	    }
	    
	    NPs.AddRange(sg_det_no_adj);
	    NPs.AddRange(pl_det_no_adj);
	    NPs.AddRange(sg_det_adj);
	    NPs.AddRange(pl_det_adj);
	    NPs.AddRange(sg_adj_no_det);
	    NPs.AddRange(pl_adj_no_det);
	    return NPs;
        }
        public static void Main(string[] args)
        {
            SynCat r = new SynCat();
	    foreach (string np in r.GetNPs())
	    {
		Console.WriteLine(np);
	    }
        }
    }
}
