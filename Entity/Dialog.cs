using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog :MonoBehaviour{

    public string dname;
    public string image;
    public string context;
    public string bg;
    public Dictionary<string, List<Dialog>> options;
    public List<Dialog> others;

    public Dialog()
    {

    }
    public Dialog(string all)
    {
        options= new Dictionary<string, List<Dialog>>();
        others = new List<Dialog>();

        this.dname = all.Split(':')[0];
        this.context =all.Split(':')[1].Split('*')[0];
        
        if (all.Contains("/"))
        {
            this.image = all.Split(':')[1].Split('*')[1].Split('/')[0];
            this.bg = all.Split('/')[1];
        }
        else
        {
            this.bg = "bg1";
            this.image = all.Split(':')[1].Split('*')[1];
        }
        //SetAttributes(all);
    }
    public void SetAttributes(string all)
    {
        int startopt = all.IndexOf('>');
        int endopt = all.IndexOf('<');
        string opt = all.Substring(startopt+1, endopt-startopt-1);
        string[] dialogs = { all.Substring(0,startopt-1), all.Substring(endopt+1,all.Length-1-endopt-1)};
        //添加共通Dialog
        for (int i = 0; i < dialogs.Length; i++)
        {
            string[] ds = dialogs[i].Split('#');
            for (int j = 0; j < ds.Length; j++)
            {
                Dialog d = new Dialog();
                d.dname = ds[i].Split(':')[0];
                d.context = ds[i].Split(':')[1].Split('*')[0];
                d.image = ds[i].Split(':')[1].Split('*')[1];
                others.Add(d);
            }
        }
        //添加选项
        string[] opts = opt.Split('/');
        for (int i = 0; i < opts.Length; i++)
        {
            int startSingle = opts[i].IndexOf('[');
            int endSingle = opts[i].IndexOf(']');
            string optDialog = opts[i].Substring(startSingle+1,endSingle-1-startSingle);
            string optStr = opts[i].Substring(0,startSingle-1);

            List<Dialog> temp = new List<Dialog>();
            string[] optDialogs = optDialog.Split('#');
            for (int j = 0; j < optDialogs.Length; j++)
            {
                Dialog d = new Dialog();
                d.dname = optDialogs[j].Split(':')[0];
                d.context = optDialogs[j].Split(':')[1].Split('*')[0];
                d.image = optDialogs[j].Split(':')[1].Split('*')[1];
                temp.Add(d);
            }
            options.Add(optStr,temp);
        }

    }
}
