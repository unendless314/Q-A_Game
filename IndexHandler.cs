using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MayCharm.Wireframe;
using System;

public class IndexHandler : GameModeController
{
    public Button button_PK;
    public Button button_School;
    public Button button_Profile;
    public Button button_Favorites;
    public Button button_LoginReward;
    public Button button_Achievements;
    public Button button_Skills;
    public Button button_MailBox;
    public Button button_Friends;
    public Button button_Shop;
    public Button button_System;


    protected override void Init()
    {
        base.Init();

        if (button_PK != null)
        {
            button_PK.onClick.AddListener(OnMatchClick);
        }

        if (button_School != null)
        {
            button_School.onClick.AddListener(OnSchoolClick);
        }

        if (button_Profile != null)
        {
            button_Profile.onClick.AddListener(OnProfileClick);
        }

        if (button_Favorites != null)
        {
            button_Favorites.onClick.AddListener(OnFavoritesClick);
        }

        if (button_LoginReward != null)
        {
            button_LoginReward.onClick.AddListener(OnLoginRewardClick);
        }

        if (button_Achievements != null)
        {
            button_Achievements.onClick.AddListener(OnAchievementsClick);
        }

        if (button_Skills != null)
        {
            button_Skills.onClick.AddListener(OnSkillsClick);
        }

        if (button_MailBox != null)
        {
            button_MailBox.onClick.AddListener(OnMailBoxClick);
        }

        if (button_Friends != null)
        {
            button_Friends.onClick.AddListener(OnFriendsClick);
        }

        if (button_Shop != null)
        {
            button_Shop.onClick.AddListener(OnShopClick);
        }

        if (button_System != null)
        {
            button_System.onClick.AddListener(OnSystemClick);
        }
    }

    private void OnMatchClick()
    {
        CurrectGamePlayMode = GameMode.MATCH;
    }

    private void OnSchoolClick()
    {
        CurrectGamePlayMode = GameMode.SCHOOL;
    }

    private void OnProfileClick()
    {
        CurrectGamePlayMode = GameMode.PROFILE;
    }

    private void OnFavoritesClick()
    {
        CurrectGamePlayMode = GameMode.FAVORITES;
    }

    private void OnLoginRewardClick()
    {
        CurrectGamePlayMode = GameMode.LOGINREWARD;
    }

    private void OnAchievementsClick()
    {
        CurrectGamePlayMode = GameMode.ACHIEVEMENTS;
    }

    private void OnSkillsClick()
    {
        CurrectGamePlayMode = GameMode.SKILLS;
    }

    private void OnMailBoxClick()
    {
        CurrectGamePlayMode = GameMode.MAILBOX;
    }

    private void OnFriendsClick()
    {
        CurrectGamePlayMode = GameMode.FRIENDS;
    }

    private void OnShopClick()
    {
        CurrectGamePlayMode = GameMode.SHOP;
    }

    private void OnSystemClick()
    {
        CurrectGamePlayMode = GameMode.SYSTEM;
    }
}
