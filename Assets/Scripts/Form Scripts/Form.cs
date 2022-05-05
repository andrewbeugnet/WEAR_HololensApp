using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using CsvHelper;

[Serializable]
public abstract class Form
{
    abstract public Boolean Completed();

    public void WriteForm(DateTime Today)
    {
        var type = this.GetType();
        string csvPath = Path.Combine(Application.persistentDataPath, $"{type.Name}-{Today.Day}-{Today.Month}-{Today.Year}.csv");
        using (var fileStream = new FileStream(csvPath, FileMode.Create))
        using (var streamWriter = new StreamWriter(fileStream))
        using (var csv = new CsvWriter(streamWriter))
        {
            var fields = type.GetFields();
            foreach (FieldInfo field in fields)
                csv.WriteField(field.Name);
            csv.NextRecord();
            foreach (FieldInfo field in fields)
                csv.WriteField(field.GetValue(this));
        }

    }
}

[Serializable]
public class TempForm : Form//make multiple of these with different class names to make different forms
{
    public double RegulatorReading;//also works with other data types
    public double TransformerReading;
    public double AirBreakersReading;
    public double AuxillaryTransformers; 
    public double CircuitSwitchers;
    public double GSUTransformerReading;
    public double MotorOperatorReading;
    public double OilBreakers;
    public double OilReclosers;
    public double SF6GasBreakerReading;

    public override bool Completed()
    {
        //WriteCollection.Save(Path.Combine(Application.persistentDataPath, "DataForm.xml"));
        throw new NotImplementedException();
    }
}


