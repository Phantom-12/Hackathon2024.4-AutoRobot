using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionEditor : MonoBehaviour
{
    string upInst, leftInst, rightInst, upleftInst, uprightInst;
    SliderController sliderController;

    void Start()
    {
        sliderController = FindObjectOfType<SliderController>();
    }

    void UpdateSlider()
    {
        sliderController.UpdateInstruction(upInst, leftInst, rightInst, upleftInst, uprightInst);
    }

    public void UpInstUpdate(string inst)
    {
        upInst = inst;
        UpdateSlider();
    }

    public void LeftInstUpdate(string inst)
    {
        leftInst = inst;
        UpdateSlider();
    }

    public void RightInstUpdate(string inst)
    {
        rightInst = inst;
        UpdateSlider();
    }

    public void UpleftInstUpdate(string inst)
    {
        upleftInst = inst;
        UpdateSlider();
    }

    public void UprightInstUpdate(string inst)
    {
        uprightInst = inst;
        UpdateSlider();
    }
}
