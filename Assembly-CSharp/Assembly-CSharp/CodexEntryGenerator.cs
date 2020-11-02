// Decompiled with JetBrains decompiler
// Type: CodexEntryGenerator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class CodexEntryGenerator
{
  public static Dictionary<string, CodexEntry> GenerateBuildingEntries()
  {
    string str1 = "BUILD_CATEGORY_";
    Dictionary<string, CodexEntry> categoryEntries = new Dictionary<string, CodexEntry>();
    foreach (PlanScreen.PlanInfo planInfo in TUNING.BUILDINGS.PLANORDER)
    {
      string str2 = HashCache.Get().Get(planInfo.category);
      string str3 = CodexCache.FormatLinkID(str1 + str2);
      Dictionary<string, CodexEntry> entries = new Dictionary<string, CodexEntry>();
      for (int index = 0; index < (planInfo.data as IList<string>).Count; ++index)
      {
        BuildingDef buildingDef = Assets.GetBuildingDef((planInfo.data as IList<string>)[index]);
        if (!buildingDef.DebugOnly)
        {
          List<ContentContainer> contentContainerList = new List<ContentContainer>();
          List<ICodexWidget> content = new List<ICodexWidget>();
          content.Add((ICodexWidget) new CodexText(buildingDef.Name, CodexTextStyle.Title));
          Tech tech = Db.Get().TechItems.LookupGroupForID(buildingDef.PrefabID);
          if (tech != null)
            content.Add((ICodexWidget) new CodexLabelWithIcon(tech.Name, CodexTextStyle.Body, new Tuple<Sprite, Color>(Assets.GetSprite((HashedString) "research_type_alpha_icon"), Color.white)));
          content.Add((ICodexWidget) new CodexDividerLine());
          contentContainerList.Add(new ContentContainer(content, ContentContainer.ContentLayout.Vertical));
          CodexEntryGenerator.GenerateImageContainers(buildingDef.GetUISprite(), contentContainerList);
          CodexEntryGenerator.GenerateBuildingDescriptionContainers(buildingDef, contentContainerList);
          CodexEntryGenerator.GenerateFabricatorContainers(buildingDef.BuildingComplete, contentContainerList);
          CodexEntryGenerator.GenerateReceptacleContainers(buildingDef.BuildingComplete, contentContainerList);
          CodexEntry entry = new CodexEntry(str3, contentContainerList, (string) Strings.Get("STRINGS.BUILDINGS.PREFABS." + (planInfo.data as IList<string>)[index].ToUpper() + ".NAME"));
          entry.icon = buildingDef.GetUISprite();
          entry.parentId = str3;
          CodexCache.AddEntry((planInfo.data as IList<string>)[index], entry);
          entries.Add(entry.id, entry);
        }
      }
      CategoryEntry categoryEntry = CodexEntryGenerator.GenerateCategoryEntry(CodexCache.FormatLinkID(str3), (string) Strings.Get("STRINGS.UI.BUILDCATEGORIES." + str2.ToUpper() + ".NAME"), entries);
      categoryEntry.parentId = "BUILDINGS";
      categoryEntry.category = "BUILDINGS";
      categoryEntry.icon = Assets.GetSprite((HashedString) PlanScreen.IconNameMap[(HashedString) str2]);
      categoryEntries.Add(str3, (CodexEntry) categoryEntry);
    }
    CodexEntryGenerator.PopulateCategoryEntries(categoryEntries);
    return categoryEntries;
  }

  public static void GeneratePageNotFound() => CodexCache.AddEntry("PageNotFound", new CodexEntry("ROOT", new List<ContentContainer>()
  {
    new ContentContainer()
    {
      content = {
        (ICodexWidget) new CodexText((string) CODEX.PAGENOTFOUND.TITLE, CodexTextStyle.Title),
        (ICodexWidget) new CodexText((string) CODEX.PAGENOTFOUND.SUBTITLE, CodexTextStyle.Subtitle),
        (ICodexWidget) new CodexDividerLine(),
        (ICodexWidget) new CodexImage(312, 312, Assets.GetSprite((HashedString) "outhouseMessage"))
      }
    }
  }, (string) CODEX.PAGENOTFOUND.TITLE)
  {
    searchOnly = true
  });

  public static Dictionary<string, CodexEntry> GenerateCreatureEntries()
  {
    Dictionary<string, CodexEntry> results = new Dictionary<string, CodexEntry>();
    List<GameObject> brains = Assets.GetPrefabsWithComponent<CreatureBrain>();
    System.Action<Tag, string> action = (System.Action<Tag, string>) ((speciesTag, name) =>
    {
      CodexEntry entry = new CodexEntry("CREATURES", new List<ContentContainer>()
      {
        new ContentContainer(new List<ICodexWidget>()
        {
          (ICodexWidget) new CodexSpacer(),
          (ICodexWidget) new CodexSpacer()
        }, ContentContainer.ContentLayout.Vertical)
      }, name);
      entry.parentId = "CREATURES";
      CodexCache.AddEntry(speciesTag.ToString(), entry);
      results.Add(speciesTag.ToString(), entry);
      foreach (GameObject gameObject in brains)
      {
        if (gameObject.GetDef<BabyMonitor.Def>() == null)
        {
          Sprite sprite = (Sprite) null;
          GameObject prefab = Assets.TryGetPrefab((Tag) (gameObject.PrefabID().ToString() + "Baby"));
          if ((UnityEngine.Object) prefab != (UnityEngine.Object) null)
            sprite = Def.GetUISprite((object) prefab).first;
          CreatureBrain component = gameObject.GetComponent<CreatureBrain>();
          if (!(component.species != speciesTag))
          {
            List<ContentContainer> contentContainerList = new List<ContentContainer>();
            string symbolPrefix = component.symbolPrefix;
            Sprite first = Def.GetUISprite((object) gameObject, symbolPrefix + "ui").first;
            if ((bool) (UnityEngine.Object) sprite)
              CodexEntryGenerator.GenerateImageContainers(new Sprite[2]
              {
                first,
                sprite
              }, contentContainerList, ContentContainer.ContentLayout.Horizontal);
            else
              CodexEntryGenerator.GenerateImageContainers(first, contentContainerList);
            CodexEntryGenerator.GenerateCreatureDescriptionContainers(gameObject, contentContainerList);
            entry.subEntries.Add(new SubEntry(component.PrefabID().ToString(), speciesTag.ToString(), contentContainerList, component.GetProperName())
            {
              icon = first,
              iconColor = Color.white
            });
          }
        }
      }
    });
    action(GameTags.Creatures.Species.PuftSpecies, (string) STRINGS.CREATURES.FAMILY_PLURAL.PUFTSPECIES);
    action(GameTags.Creatures.Species.PacuSpecies, (string) STRINGS.CREATURES.FAMILY_PLURAL.PACUSPECIES);
    action(GameTags.Creatures.Species.OilFloaterSpecies, (string) STRINGS.CREATURES.FAMILY_PLURAL.OILFLOATERSPECIES);
    action(GameTags.Creatures.Species.LightBugSpecies, (string) STRINGS.CREATURES.FAMILY_PLURAL.LIGHTBUGSPECIES);
    action(GameTags.Creatures.Species.HatchSpecies, (string) STRINGS.CREATURES.FAMILY_PLURAL.HATCHSPECIES);
    action(GameTags.Creatures.Species.GlomSpecies, (string) STRINGS.CREATURES.FAMILY_PLURAL.GLOMSPECIES);
    action(GameTags.Creatures.Species.DreckoSpecies, (string) STRINGS.CREATURES.FAMILY_PLURAL.DRECKOSPECIES);
    action(GameTags.Creatures.Species.MooSpecies, (string) STRINGS.CREATURES.FAMILY_PLURAL.MOOSPECIES);
    action(GameTags.Creatures.Species.MoleSpecies, (string) STRINGS.CREATURES.FAMILY_PLURAL.MOLESPECIES);
    action(GameTags.Creatures.Species.SquirrelSpecies, (string) STRINGS.CREATURES.FAMILY_PLURAL.SQUIRRELSPECIES);
    action(GameTags.Creatures.Species.CrabSpecies, (string) STRINGS.CREATURES.FAMILY_PLURAL.CRABSPECIES);
    action(GameTags.Robots.Models.SweepBot, (string) ROBOTS.CATEGORY_NAME);
    return results;
  }

  public static Dictionary<string, CodexEntry> GeneratePlantEntries()
  {
    Dictionary<string, CodexEntry> dictionary1 = new Dictionary<string, CodexEntry>();
    List<GameObject> prefabsWithComponent = Assets.GetPrefabsWithComponent<Harvestable>();
    prefabsWithComponent.AddRange((IEnumerable<GameObject>) Assets.GetPrefabsWithComponent<WiltCondition>());
    foreach (GameObject gameObject in prefabsWithComponent)
    {
      Dictionary<string, CodexEntry> dictionary2 = dictionary1;
      Tag tag = gameObject.PrefabID();
      string key1 = tag.ToString();
      if (!dictionary2.ContainsKey(key1) && !((UnityEngine.Object) gameObject.GetComponent<BudUprootedMonitor>() != (UnityEngine.Object) null))
      {
        List<ContentContainer> contentContainerList = new List<ContentContainer>();
        Sprite first = Def.GetUISprite((object) gameObject).first;
        CodexEntryGenerator.GenerateImageContainers(first, contentContainerList);
        CodexEntryGenerator.GeneratePlantDescriptionContainers(gameObject, contentContainerList);
        CodexEntry entry = new CodexEntry("PLANTS", contentContainerList, gameObject.GetProperName());
        entry.parentId = "PLANTS";
        entry.icon = first;
        tag = gameObject.PrefabID();
        CodexCache.AddEntry(tag.ToString(), entry);
        Dictionary<string, CodexEntry> dictionary3 = dictionary1;
        tag = gameObject.PrefabID();
        string key2 = tag.ToString();
        CodexEntry codexEntry = entry;
        dictionary3.Add(key2, codexEntry);
      }
    }
    return dictionary1;
  }

  public static Dictionary<string, CodexEntry> GenerateFoodEntries()
  {
    Dictionary<string, CodexEntry> dictionary = new Dictionary<string, CodexEntry>();
    foreach (EdiblesManager.FoodInfo foodTypes in TUNING.FOOD.FOOD_TYPES_LIST)
    {
      if (!Assets.GetPrefab((Tag) foodTypes.Id).HasTag(GameTags.IncubatableEgg))
      {
        List<ContentContainer> contentContainerList = new List<ContentContainer>();
        CodexEntryGenerator.GenerateTitleContainers(foodTypes.Name, contentContainerList);
        Sprite first = Def.GetUISprite((object) foodTypes.ConsumableId).first;
        CodexEntryGenerator.GenerateImageContainers(first, contentContainerList);
        CodexEntryGenerator.GenerateFoodDescriptionContainers(foodTypes, contentContainerList);
        CodexEntryGenerator.GenerateRecipeContainers(foodTypes.ConsumableId.ToTag(), contentContainerList);
        CodexEntryGenerator.GenerateUsedInRecipeContainers(foodTypes.ConsumableId.ToTag(), contentContainerList);
        CodexEntry entry = new CodexEntry("FOOD", contentContainerList, foodTypes.Name);
        entry.icon = first;
        entry.parentId = "FOOD";
        CodexCache.AddEntry(foodTypes.Id, entry);
        dictionary.Add(foodTypes.Id, entry);
      }
    }
    return dictionary;
  }

  public static Dictionary<string, CodexEntry> GenerateTechEntries()
  {
    Dictionary<string, CodexEntry> dictionary = new Dictionary<string, CodexEntry>();
    foreach (Tech resource in Db.Get().Techs.resources)
    {
      List<ContentContainer> contentContainerList = new List<ContentContainer>();
      CodexEntryGenerator.GenerateTitleContainers(resource.Name, contentContainerList);
      CodexEntryGenerator.GenerateTechDescriptionContainers(resource, contentContainerList);
      CodexEntryGenerator.GeneratePrerequisiteTechContainers(resource, contentContainerList);
      CodexEntryGenerator.GenerateUnlockContainers(resource, contentContainerList);
      CodexEntry entry = new CodexEntry("TECH", contentContainerList, resource.Name);
      TechItem unlockedItem = resource.unlockedItems[0];
      if (unlockedItem == null)
        DebugUtil.LogErrorArgs((object) "Unknown tech:", (object) resource.Name);
      entry.icon = unlockedItem.getUISprite("ui", false);
      entry.parentId = "TECH";
      CodexCache.AddEntry(resource.Id, entry);
      dictionary.Add(resource.Id, entry);
    }
    return dictionary;
  }

  public static Dictionary<string, CodexEntry> GenerateRoleEntries()
  {
    Dictionary<string, CodexEntry> dictionary = new Dictionary<string, CodexEntry>();
    foreach (Skill resource in Db.Get().Skills.resources)
    {
      List<ContentContainer> contentContainerList = new List<ContentContainer>();
      Sprite sprite = Assets.GetSprite((HashedString) resource.hat);
      CodexEntryGenerator.GenerateTitleContainers(resource.Name, contentContainerList);
      CodexEntryGenerator.GenerateImageContainers(sprite, contentContainerList);
      CodexEntryGenerator.GenerateGenericDescriptionContainers(resource.description, contentContainerList);
      CodexEntryGenerator.GenerateSkillRequirementsAndPerksContainers(resource, contentContainerList);
      CodexEntryGenerator.GenerateRelatedSkillContainers(resource, contentContainerList);
      CodexEntry entry = new CodexEntry("ROLES", contentContainerList, resource.Name);
      entry.parentId = "ROLES";
      entry.icon = sprite;
      CodexCache.AddEntry(resource.Id, entry);
      dictionary.Add(resource.Id, entry);
    }
    return dictionary;
  }

  public static Dictionary<string, CodexEntry> GenerateGeyserEntries()
  {
    Dictionary<string, CodexEntry> dictionary = new Dictionary<string, CodexEntry>();
    List<GameObject> prefabsWithComponent = Assets.GetPrefabsWithComponent<Geyser>();
    if (prefabsWithComponent != null)
    {
      foreach (GameObject go in prefabsWithComponent)
      {
        if (!go.GetComponent<KPrefabID>().HasTag(GameTags.DeprecatedContent))
        {
          List<ContentContainer> contentContainerList = new List<ContentContainer>();
          CodexEntryGenerator.GenerateTitleContainers(go.GetProperName(), contentContainerList);
          Sprite first = Def.GetUISprite((object) go).first;
          CodexEntryGenerator.GenerateImageContainers(first, contentContainerList);
          List<ICodexWidget> content = new List<ICodexWidget>();
          Tag tag = go.PrefabID();
          content.Add((ICodexWidget) new CodexText((string) Strings.Get("STRINGS.CREATURES.SPECIES.GEYSER." + tag.ToString().Remove(0, 14).ToUpper() + ".DESC")));
          content.Add((ICodexWidget) new CodexText((string) UI.CODEX.GEYSERS.DESC));
          ContentContainer contentContainer = new ContentContainer(content, ContentContainer.ContentLayout.Vertical);
          contentContainerList.Add(contentContainer);
          CodexEntry entry = new CodexEntry("GEYSERS", contentContainerList, go.GetProperName());
          entry.icon = first;
          entry.parentId = "GEYSERS";
          CodexEntry codexEntry = entry;
          tag = go.PrefabID();
          string str = tag.ToString();
          codexEntry.id = str;
          CodexCache.AddEntry(entry.id, entry);
          dictionary.Add(entry.id, entry);
        }
      }
    }
    return dictionary;
  }

  public static Dictionary<string, CodexEntry> GenerateEquipmentEntries()
  {
    Dictionary<string, CodexEntry> dictionary = new Dictionary<string, CodexEntry>();
    List<GameObject> prefabsWithComponent = Assets.GetPrefabsWithComponent<Equippable>();
    if (prefabsWithComponent != null)
    {
      foreach (GameObject go in prefabsWithComponent)
      {
        bool flag = false;
        Equippable component = go.GetComponent<Equippable>();
        if (component.def.AdditionalTags != null)
        {
          foreach (Tag additionalTag in component.def.AdditionalTags)
          {
            if (additionalTag == GameTags.DeprecatedContent)
            {
              flag = true;
              break;
            }
          }
        }
        if (!flag && !component.hideInCodex)
        {
          List<ContentContainer> contentContainerList = new List<ContentContainer>();
          CodexEntryGenerator.GenerateTitleContainers(go.GetProperName(), contentContainerList);
          Sprite first = Def.GetUISprite((object) go).first;
          CodexEntryGenerator.GenerateImageContainers(first, contentContainerList);
          ContentContainer contentContainer = new ContentContainer(new List<ICodexWidget>()
          {
            (ICodexWidget) new CodexText((string) Strings.Get("STRINGS.EQUIPMENT.PREFABS." + go.PrefabID().ToString().ToUpper() + ".DESC"))
          }, ContentContainer.ContentLayout.Vertical);
          contentContainerList.Add(contentContainer);
          CodexEntry entry = new CodexEntry("EQUIPMENT", contentContainerList, go.GetProperName());
          entry.icon = first;
          entry.parentId = "EQUIPMENT";
          entry.id = go.PrefabID().ToString();
          CodexCache.AddEntry(entry.id, entry);
          dictionary.Add(entry.id, entry);
        }
      }
    }
    return dictionary;
  }

  public static Dictionary<string, CodexEntry> GenerateElementEntries()
  {
    Dictionary<string, CodexEntry> categoryEntries = new Dictionary<string, CodexEntry>();
    Dictionary<string, CodexEntry> entries1 = new Dictionary<string, CodexEntry>();
    Dictionary<string, CodexEntry> entries2 = new Dictionary<string, CodexEntry>();
    Dictionary<string, CodexEntry> entries3 = new Dictionary<string, CodexEntry>();
    Dictionary<string, CodexEntry> entries4 = new Dictionary<string, CodexEntry>();
    string str1 = CodexCache.FormatLinkID("ELEMENTS");
    string str2 = CodexCache.FormatLinkID("ELEMENTS_SOLID");
    string str3 = CodexCache.FormatLinkID("ELEMENTS_LIQUID");
    string str4 = CodexCache.FormatLinkID("ELEMENTS_GAS");
    string str5 = CodexCache.FormatLinkID("ELEMENTS_OTHER");
    System.Action<Element, List<ContentContainer>> action = (System.Action<Element, List<ContentContainer>>) ((element, containers) =>
    {
      if (element.highTempTransition != null || element.lowTempTransition != null)
        containers.Add(new ContentContainer(new List<ICodexWidget>()
        {
          (ICodexWidget) new CodexText((string) CODEX.HEADERS.ELEMENTTRANSITIONS, CodexTextStyle.Subtitle),
          (ICodexWidget) new CodexDividerLine()
        }, ContentContainer.ContentLayout.Vertical));
      if (element.highTempTransition != null)
      {
        List<ContentContainer> contentContainerList = containers;
        List<ICodexWidget> content = new List<ICodexWidget>();
        content.Add((ICodexWidget) new CodexImage(32, 32, Def.GetUISprite((object) element.highTempTransition)));
        List<ICodexWidget> codexWidgetList = content;
        string text;
        if (element.highTempTransition == null)
          text = "";
        else
          text = element.highTempTransition.name + " (" + element.highTempTransition.GetStateString() + ")  (" + GameUtil.GetFormattedTemperature(element.highTemp) + ")";
        CodexText codexText = new CodexText(text);
        codexWidgetList.Add((ICodexWidget) codexText);
        ContentContainer contentContainer = new ContentContainer(content, ContentContainer.ContentLayout.Horizontal);
        contentContainerList.Add(contentContainer);
      }
      if (element.lowTempTransition != null)
      {
        List<ContentContainer> contentContainerList = containers;
        List<ICodexWidget> content = new List<ICodexWidget>();
        content.Add((ICodexWidget) new CodexImage(32, 32, Def.GetUISprite((object) element.lowTempTransition)));
        List<ICodexWidget> codexWidgetList = content;
        string text;
        if (element.lowTempTransition == null)
          text = "";
        else
          text = element.lowTempTransition.name + " (" + element.lowTempTransition.GetStateString() + ")  (" + GameUtil.GetFormattedTemperature(element.lowTemp) + ")";
        CodexText codexText = new CodexText(text);
        codexWidgetList.Add((ICodexWidget) codexText);
        ContentContainer contentContainer = new ContentContainer(content, ContentContainer.ContentLayout.Horizontal);
        contentContainerList.Add(contentContainer);
      }
      containers.Add(new ContentContainer(new List<ICodexWidget>()
      {
        (ICodexWidget) new CodexSpacer(),
        (ICodexWidget) new CodexText(element.FullDescription()),
        (ICodexWidget) new CodexSpacer()
      }, ContentContainer.ContentLayout.Vertical));
    });
    foreach (Element element in ElementLoader.elements)
    {
      if (!element.disabled)
      {
        List<ContentContainer> contentContainerList = new List<ContentContainer>();
        string name = element.name + " (" + element.GetStateString() + ")";
        Tuple<Sprite, Color> tuple = Def.GetUISprite((object) element);
        if ((UnityEngine.Object) tuple.first == (UnityEngine.Object) null)
        {
          if (element.id == SimHashes.Void)
          {
            name = element.name;
            tuple = new Tuple<Sprite, Color>(Assets.GetSprite((HashedString) "ui_elements-void"), Color.white);
          }
          else if (element.id == SimHashes.Vacuum)
          {
            name = element.name;
            tuple = new Tuple<Sprite, Color>(Assets.GetSprite((HashedString) "ui_elements-vacuum"), Color.white);
          }
        }
        CodexEntryGenerator.GenerateTitleContainers(name, contentContainerList);
        CodexEntryGenerator.GenerateImageContainers(new Tuple<Sprite, Color>[1]
        {
          tuple
        }, contentContainerList, ContentContainer.ContentLayout.Horizontal);
        action(element, contentContainerList);
        string str6 = element.id.ToString();
        string category;
        Dictionary<string, CodexEntry> dictionary;
        if (element.IsSolid)
        {
          category = str2;
          dictionary = entries1;
        }
        else if (element.IsLiquid)
        {
          category = str3;
          dictionary = entries2;
        }
        else if (element.IsGas)
        {
          category = str4;
          dictionary = entries3;
        }
        else
        {
          category = str5;
          dictionary = entries4;
        }
        CodexEntry entry = new CodexEntry(category, contentContainerList, name);
        entry.parentId = category;
        entry.icon = tuple.first;
        entry.iconColor = tuple.second;
        CodexCache.AddEntry(str6, entry);
        dictionary.Add(str6, entry);
      }
    }
    string str7 = str2;
    CodexEntry categoryEntry1 = (CodexEntry) CodexEntryGenerator.GenerateCategoryEntry(str7, (string) UI.CODEX.CATEGORYNAMES.ELEMENTSSOLID, entries1, Assets.GetSprite((HashedString) "ui_elements-solid"));
    categoryEntry1.parentId = str1;
    categoryEntry1.category = str1;
    categoryEntries.Add(str7, categoryEntry1);
    string str8 = str3;
    CodexEntry categoryEntry2 = (CodexEntry) CodexEntryGenerator.GenerateCategoryEntry(str8, (string) UI.CODEX.CATEGORYNAMES.ELEMENTSLIQUID, entries2, Assets.GetSprite((HashedString) "ui_elements-liquids"));
    categoryEntry2.parentId = str1;
    categoryEntry2.category = str1;
    categoryEntries.Add(str8, categoryEntry2);
    string str9 = str4;
    CodexEntry categoryEntry3 = (CodexEntry) CodexEntryGenerator.GenerateCategoryEntry(str9, (string) UI.CODEX.CATEGORYNAMES.ELEMENTSGAS, entries3, Assets.GetSprite((HashedString) "ui_elements-gases"));
    categoryEntry3.parentId = str1;
    categoryEntry3.category = str1;
    categoryEntries.Add(str9, categoryEntry3);
    string str10 = str5;
    CodexEntry categoryEntry4 = (CodexEntry) CodexEntryGenerator.GenerateCategoryEntry(str10, (string) UI.CODEX.CATEGORYNAMES.ELEMENTSOTHER, entries4, Assets.GetSprite((HashedString) "ui_elements-other"));
    categoryEntry4.parentId = str1;
    categoryEntry4.category = str1;
    categoryEntries.Add(str10, categoryEntry4);
    CodexEntryGenerator.PopulateCategoryEntries(categoryEntries);
    return categoryEntries;
  }

  public static Dictionary<string, CodexEntry> GenerateDiseaseEntries()
  {
    Dictionary<string, CodexEntry> dictionary = new Dictionary<string, CodexEntry>();
    foreach (Klei.AI.Disease resource in Db.Get().Diseases.resources)
    {
      if (!resource.Disabled)
      {
        List<ContentContainer> contentContainerList = new List<ContentContainer>();
        CodexEntryGenerator.GenerateTitleContainers(resource.Name, contentContainerList);
        CodexEntryGenerator.GenerateDiseaseDescriptionContainers(resource, contentContainerList);
        CodexEntry entry = new CodexEntry("DISEASE", contentContainerList, resource.Name);
        entry.parentId = "DISEASE";
        dictionary.Add(resource.Id, entry);
        entry.icon = Assets.GetSprite((HashedString) "overlay_disease");
        CodexCache.AddEntry(resource.Id, entry);
      }
    }
    return dictionary;
  }

  public static CategoryEntry GenerateCategoryEntry(
    string id,
    string name,
    Dictionary<string, CodexEntry> entries,
    Sprite icon = null,
    bool largeFormat = true,
    bool sort = true,
    string overrideHeader = null)
  {
    List<ContentContainer> contentContainerList = new List<ContentContainer>();
    CodexEntryGenerator.GenerateTitleContainers(overrideHeader == null ? name : overrideHeader, contentContainerList);
    List<CodexEntry> entriesInCategory = new List<CodexEntry>();
    foreach (KeyValuePair<string, CodexEntry> entry in entries)
    {
      entriesInCategory.Add(entry.Value);
      if ((UnityEngine.Object) icon == (UnityEngine.Object) null)
        icon = entry.Value.icon;
    }
    CategoryEntry categoryEntry = new CategoryEntry("Root", contentContainerList, name, entriesInCategory, largeFormat, sort);
    categoryEntry.icon = icon;
    CodexCache.AddEntry(id, (CodexEntry) categoryEntry);
    return categoryEntry;
  }

  public static Dictionary<string, CodexEntry> GenerateTutorialNotificationEntries()
  {
    CodexEntry entry1 = new CodexEntry("MISCELLANEOUSTIPS", new List<ContentContainer>()
    {
      new ContentContainer(new List<ICodexWidget>()
      {
        (ICodexWidget) new CodexSpacer()
      }, ContentContainer.ContentLayout.Vertical)
    }, (string) Strings.Get("STRINGS.UI.CODEX.CATEGORYNAMES.MISCELLANEOUSTIPS"));
    Dictionary<string, CodexEntry> dictionary = new Dictionary<string, CodexEntry>();
    for (int index = 0; index < 19; ++index)
    {
      TutorialMessage tutorialMessage = (TutorialMessage) Tutorial.Instance.TutorialMessage((Tutorial.TutorialMessages) index, false);
      if (tutorialMessage != null)
      {
        if (!string.IsNullOrEmpty(tutorialMessage.videoClipId))
        {
          List<ContentContainer> contentContainerList = new List<ContentContainer>();
          CodexEntryGenerator.GenerateTitleContainers(tutorialMessage.GetTitle(), contentContainerList);
          contentContainerList.Add(new ContentContainer(new List<ICodexWidget>()
          {
            (ICodexWidget) new CodexVideo()
            {
              videoName = tutorialMessage.videoClipId,
              overlayName = tutorialMessage.videoOverlayName,
              overlayTexts = new List<string>()
              {
                tutorialMessage.videoTitleText,
                (string) VIDEOS.TUTORIAL_HEADER
              }
            }
          }, ContentContainer.ContentLayout.Vertical));
          contentContainerList.Add(new ContentContainer(new List<ICodexWidget>()
          {
            (ICodexWidget) new CodexText(tutorialMessage.GetMessageBody())
          }, ContentContainer.ContentLayout.Vertical));
          CodexEntry entry2 = new CodexEntry("Videos", contentContainerList, UI.FormatAsLink(tutorialMessage.GetTitle(), "videos_" + (object) index));
          entry2.icon = Assets.GetSprite((HashedString) "codexVideo");
          CodexCache.AddEntry("videos_" + (object) index, entry2);
          dictionary.Add(entry2.id, entry2);
        }
        else
        {
          List<ContentContainer> contentContainerList = new List<ContentContainer>();
          CodexEntryGenerator.GenerateTitleContainers(tutorialMessage.GetTitle(), contentContainerList);
          contentContainerList.Add(new ContentContainer(new List<ICodexWidget>()
          {
            (ICodexWidget) new CodexText(tutorialMessage.GetMessageBody())
          }, ContentContainer.ContentLayout.Vertical));
          contentContainerList.Add(new ContentContainer(new List<ICodexWidget>()
          {
            (ICodexWidget) new CodexSpacer(),
            (ICodexWidget) new CodexSpacer()
          }, ContentContainer.ContentLayout.Vertical));
          SubEntry subEntry = new SubEntry("MISCELLANEOUSTIPS" + (object) index, "MISCELLANEOUSTIPS", contentContainerList, tutorialMessage.GetTitle());
          entry1.subEntries.Add(subEntry);
        }
      }
    }
    CodexCache.AddEntry("MISCELLANEOUSTIPS", entry1);
    return dictionary;
  }

  public static void PopulateCategoryEntries(Dictionary<string, CodexEntry> categoryEntries)
  {
    List<CategoryEntry> categoryEntries1 = new List<CategoryEntry>();
    foreach (KeyValuePair<string, CodexEntry> categoryEntry in categoryEntries)
      categoryEntries1.Add(categoryEntry.Value as CategoryEntry);
    CodexEntryGenerator.PopulateCategoryEntries(categoryEntries1);
  }

  public static void PopulateCategoryEntries(
    List<CategoryEntry> categoryEntries,
    Comparison<CodexEntry> comparison = null)
  {
    foreach (CategoryEntry categoryEntry in categoryEntries)
    {
      List<ContentContainer> contentContainers = categoryEntry.contentContainers;
      List<CodexEntry> codexEntryList = new List<CodexEntry>();
      foreach (CodexEntry codexEntry in categoryEntry.entriesInCategory)
        codexEntryList.Add(codexEntry);
      if (categoryEntry.sort)
      {
        if (comparison == null)
          codexEntryList.Sort((Comparison<CodexEntry>) ((a, b) => UI.StripLinkFormatting(a.name).CompareTo(UI.StripLinkFormatting(b.name))));
        else
          codexEntryList.Sort(comparison);
      }
      if (categoryEntry.largeFormat)
      {
        ContentContainer contentContainer1 = new ContentContainer(new List<ICodexWidget>(), ContentContainer.ContentLayout.Grid);
        foreach (CodexEntry codexEntry in codexEntryList)
          contentContainer1.content.Add((ICodexWidget) new CodexLabelWithLargeIcon(codexEntry.name, CodexTextStyle.BodyWhite, new Tuple<Sprite, Color>((UnityEngine.Object) codexEntry.icon != (UnityEngine.Object) null ? codexEntry.icon : Assets.GetSprite((HashedString) "unknown"), codexEntry.iconColor), codexEntry.id));
        if (categoryEntry.showBeforeGeneratedCategoryLinks)
        {
          contentContainers.Add(contentContainer1);
        }
        else
        {
          ContentContainer contentContainer2 = contentContainers[contentContainers.Count - 1];
          contentContainers.RemoveAt(contentContainers.Count - 1);
          contentContainers.Insert(0, contentContainer2);
          contentContainers.Insert(1, contentContainer1);
          contentContainers.Insert(2, new ContentContainer(new List<ICodexWidget>()
          {
            (ICodexWidget) new CodexSpacer()
          }, ContentContainer.ContentLayout.Vertical));
        }
      }
      else
      {
        ContentContainer contentContainer1 = new ContentContainer(new List<ICodexWidget>(), ContentContainer.ContentLayout.Vertical);
        foreach (CodexEntry codexEntry in codexEntryList)
        {
          if ((UnityEngine.Object) codexEntry.icon == (UnityEngine.Object) null)
            contentContainer1.content.Add((ICodexWidget) new CodexText(codexEntry.name));
          else
            contentContainer1.content.Add((ICodexWidget) new CodexLabelWithIcon(codexEntry.name, CodexTextStyle.Body, new Tuple<Sprite, Color>(codexEntry.icon, codexEntry.iconColor), 64, 48));
        }
        if (categoryEntry.showBeforeGeneratedCategoryLinks)
        {
          contentContainers.Add(contentContainer1);
        }
        else
        {
          ContentContainer contentContainer2 = contentContainers[contentContainers.Count - 1];
          contentContainers.RemoveAt(contentContainers.Count - 1);
          contentContainers.Insert(0, contentContainer2);
          contentContainers.Insert(1, contentContainer1);
        }
      }
    }
  }

  private static void GenerateTitleContainers(string name, List<ContentContainer> containers) => containers.Add(new ContentContainer(new List<ICodexWidget>()
  {
    (ICodexWidget) new CodexText(name, CodexTextStyle.Title),
    (ICodexWidget) new CodexDividerLine()
  }, ContentContainer.ContentLayout.Vertical));

  private static void GeneratePrerequisiteTechContainers(
    Tech tech,
    List<ContentContainer> containers)
  {
    if (tech.requiredTech == null || tech.requiredTech.Count == 0)
      return;
    List<ICodexWidget> content = new List<ICodexWidget>();
    content.Add((ICodexWidget) new CodexText((string) CODEX.HEADERS.PREREQUISITE_TECH, CodexTextStyle.Subtitle));
    content.Add((ICodexWidget) new CodexDividerLine());
    content.Add((ICodexWidget) new CodexSpacer());
    foreach (Tech tech1 in tech.requiredTech)
      content.Add((ICodexWidget) new CodexText(tech1.Name));
    content.Add((ICodexWidget) new CodexSpacer());
    containers.Add(new ContentContainer(content, ContentContainer.ContentLayout.Vertical));
  }

  private static void GenerateSkillRequirementsAndPerksContainers(
    Skill skill,
    List<ContentContainer> containers)
  {
    List<ICodexWidget> content = new List<ICodexWidget>();
    CodexText codexText1 = new CodexText((string) CODEX.HEADERS.ROLE_PERKS, CodexTextStyle.Subtitle);
    CodexText codexText2 = new CodexText((string) CODEX.HEADERS.ROLE_PERKS_DESC);
    content.Add((ICodexWidget) codexText1);
    content.Add((ICodexWidget) new CodexDividerLine());
    content.Add((ICodexWidget) codexText2);
    content.Add((ICodexWidget) new CodexSpacer());
    foreach (Resource perk in skill.perks)
    {
      CodexText codexText3 = new CodexText(perk.Name);
      content.Add((ICodexWidget) codexText3);
    }
    containers.Add(new ContentContainer(content, ContentContainer.ContentLayout.Vertical));
    content.Add((ICodexWidget) new CodexSpacer());
  }

  private static void GenerateRelatedSkillContainers(Skill skill, List<ContentContainer> containers)
  {
    bool flag1 = false;
    List<ICodexWidget> content1 = new List<ICodexWidget>();
    content1.Add((ICodexWidget) new CodexText((string) CODEX.HEADERS.PREREQUISITE_ROLES, CodexTextStyle.Subtitle));
    content1.Add((ICodexWidget) new CodexDividerLine());
    content1.Add((ICodexWidget) new CodexSpacer());
    foreach (string priorSkill in skill.priorSkills)
    {
      CodexText codexText = new CodexText(Db.Get().Skills.Get(priorSkill).Name);
      content1.Add((ICodexWidget) codexText);
      flag1 = true;
    }
    if (flag1)
    {
      content1.Add((ICodexWidget) new CodexSpacer());
      containers.Add(new ContentContainer(content1, ContentContainer.ContentLayout.Vertical));
    }
    bool flag2 = false;
    List<ICodexWidget> content2 = new List<ICodexWidget>();
    CodexText codexText1 = new CodexText((string) CODEX.HEADERS.UNLOCK_ROLES, CodexTextStyle.Subtitle);
    CodexText codexText2 = new CodexText((string) CODEX.HEADERS.UNLOCK_ROLES_DESC);
    content2.Add((ICodexWidget) codexText1);
    content2.Add((ICodexWidget) new CodexDividerLine());
    content2.Add((ICodexWidget) codexText2);
    content2.Add((ICodexWidget) new CodexSpacer());
    foreach (Skill resource in Db.Get().Skills.resources)
    {
      foreach (string priorSkill in resource.priorSkills)
      {
        if (priorSkill == skill.Id)
        {
          CodexText codexText3 = new CodexText(resource.Name);
          content2.Add((ICodexWidget) codexText3);
          flag2 = true;
        }
      }
    }
    if (!flag2)
      return;
    content2.Add((ICodexWidget) new CodexSpacer());
    containers.Add(new ContentContainer(content2, ContentContainer.ContentLayout.Vertical));
  }

  private static void GenerateUnlockContainers(Tech tech, List<ContentContainer> containers)
  {
    containers.Add(new ContentContainer(new List<ICodexWidget>()
    {
      (ICodexWidget) new CodexText((string) CODEX.HEADERS.TECH_UNLOCKS, CodexTextStyle.Subtitle),
      (ICodexWidget) new CodexDividerLine(),
      (ICodexWidget) new CodexSpacer()
    }, ContentContainer.ContentLayout.Vertical));
    foreach (TechItem unlockedItem in tech.unlockedItems)
      containers.Add(new ContentContainer(new List<ICodexWidget>()
      {
        (ICodexWidget) new CodexImage(64, 64, unlockedItem.getUISprite("ui", false)),
        (ICodexWidget) new CodexText(unlockedItem.Name)
      }, ContentContainer.ContentLayout.Horizontal));
  }

  private static void GenerateRecipeContainers(Tag prefabID, List<ContentContainer> containers)
  {
    Recipe recipe1 = (Recipe) null;
    foreach (Recipe recipe2 in RecipeManager.Get().recipes)
    {
      if (recipe2.Result == prefabID)
      {
        recipe1 = recipe2;
        break;
      }
    }
    if (recipe1 == null)
      return;
    containers.Add(new ContentContainer(new List<ICodexWidget>()
    {
      (ICodexWidget) new CodexText((string) CODEX.HEADERS.RECIPE, CodexTextStyle.Subtitle),
      (ICodexWidget) new CodexSpacer(),
      (ICodexWidget) new CodexDividerLine()
    }, ContentContainer.ContentLayout.Vertical));
    Func<Recipe, List<ContentContainer>> func = (Func<Recipe, List<ContentContainer>>) (rec =>
    {
      List<ContentContainer> contentContainerList = new List<ContentContainer>();
      foreach (Recipe.Ingredient ingredient in rec.Ingredients)
      {
        GameObject prefab = Assets.GetPrefab(ingredient.tag);
        if ((UnityEngine.Object) prefab != (UnityEngine.Object) null)
          contentContainerList.Add(new ContentContainer(new List<ICodexWidget>()
          {
            (ICodexWidget) new CodexImage(64, 64, Def.GetUISprite((object) prefab)),
            (ICodexWidget) new CodexText(string.Format((string) UI.CODEX.RECIPE_ITEM, (object) Assets.GetPrefab(ingredient.tag).GetProperName(), (object) ingredient.amount, ElementLoader.GetElement(ingredient.tag) == null ? (object) "" : (object) UI.UNITSUFFIXES.MASS.KILOGRAM.text))
          }, ContentContainer.ContentLayout.Horizontal));
      }
      return contentContainerList;
    });
    containers.AddRange((IEnumerable<ContentContainer>) func(recipe1));
    GameObject go = recipe1.fabricators == null ? (GameObject) null : Assets.GetPrefab((Tag) recipe1.fabricators[0]);
    if (!((UnityEngine.Object) go != (UnityEngine.Object) null))
      return;
    containers.Add(new ContentContainer(new List<ICodexWidget>()
    {
      (ICodexWidget) new CodexText((string) UI.CODEX.RECIPE_FABRICATOR_HEADER, CodexTextStyle.Subtitle),
      (ICodexWidget) new CodexDividerLine()
    }, ContentContainer.ContentLayout.Vertical));
    containers.Add(new ContentContainer(new List<ICodexWidget>()
    {
      (ICodexWidget) new CodexImage(64, 64, Def.GetUISpriteFromMultiObjectAnim(go.GetComponent<KBatchedAnimController>().AnimFiles[0])),
      (ICodexWidget) new CodexText(string.Format((string) UI.CODEX.RECIPE_FABRICATOR, (object) recipe1.FabricationTime, (object) go.GetProperName()))
    }, ContentContainer.ContentLayout.Horizontal));
  }

  private static void GenerateUsedInRecipeContainers(
    Tag prefabID,
    List<ContentContainer> containers)
  {
    List<Recipe> recipeList = new List<Recipe>();
    foreach (Recipe recipe in RecipeManager.Get().recipes)
    {
      foreach (Recipe.Ingredient ingredient in recipe.Ingredients)
      {
        if (ingredient.tag == prefabID)
          recipeList.Add(recipe);
      }
    }
    if (recipeList.Count == 0)
      return;
    containers.Add(new ContentContainer(new List<ICodexWidget>()
    {
      (ICodexWidget) new CodexText((string) CODEX.HEADERS.USED_IN_RECIPES, CodexTextStyle.Subtitle),
      (ICodexWidget) new CodexSpacer(),
      (ICodexWidget) new CodexDividerLine()
    }, ContentContainer.ContentLayout.Vertical));
    foreach (Recipe recipe in recipeList)
    {
      GameObject prefab = Assets.GetPrefab(recipe.Result);
      containers.Add(new ContentContainer(new List<ICodexWidget>()
      {
        (ICodexWidget) new CodexImage(64, 64, Def.GetUISprite((object) prefab)),
        (ICodexWidget) new CodexText(prefab.GetProperName())
      }, ContentContainer.ContentLayout.Horizontal));
    }
  }

  private static void GeneratePlantDescriptionContainers(
    GameObject plant,
    List<ContentContainer> containers)
  {
    SeedProducer component1 = plant.GetComponent<SeedProducer>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
    {
      GameObject prefab = Assets.GetPrefab((Tag) component1.seedInfo.seedId);
      containers.Add(new ContentContainer(new List<ICodexWidget>()
      {
        (ICodexWidget) new CodexText((string) CODEX.HEADERS.GROWNFROMSEED, CodexTextStyle.Subtitle),
        (ICodexWidget) new CodexDividerLine()
      }, ContentContainer.ContentLayout.Vertical));
      containers.Add(new ContentContainer(new List<ICodexWidget>()
      {
        (ICodexWidget) new CodexImage(48, 48, Def.GetUISprite((object) prefab)),
        (ICodexWidget) new CodexText(prefab.GetProperName())
      }, ContentContainer.ContentLayout.Horizontal));
    }
    List<ICodexWidget> content = new List<ICodexWidget>();
    content.Add((ICodexWidget) new CodexSpacer());
    content.Add((ICodexWidget) new CodexText((string) UI.CODEX.DETAILS, CodexTextStyle.Subtitle));
    content.Add((ICodexWidget) new CodexDividerLine());
    InfoDescription component2 = Assets.GetPrefab(plant.PrefabID()).GetComponent<InfoDescription>();
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
      content.Add((ICodexWidget) new CodexText(component2.description));
    string str1 = "";
    List<Descriptor> requirementDescriptors = GameUtil.GetPlantRequirementDescriptors(plant);
    if (requirementDescriptors.Count > 0)
    {
      string text = str1 + requirementDescriptors[0].text;
      for (int index = 1; index < requirementDescriptors.Count; ++index)
        text = text + "\n    • " + requirementDescriptors[index].text;
      content.Add((ICodexWidget) new CodexText(text));
      content.Add((ICodexWidget) new CodexSpacer());
    }
    string str2 = "";
    List<Descriptor> effectDescriptors = GameUtil.GetPlantEffectDescriptors(plant);
    if (effectDescriptors.Count > 0)
    {
      string text = str2 + effectDescriptors[0].text;
      for (int index = 1; index < effectDescriptors.Count; ++index)
        text = text + "\n    • " + effectDescriptors[index].text;
      CodexText codexText = new CodexText(text);
      content.Add((ICodexWidget) codexText);
      content.Add((ICodexWidget) new CodexSpacer());
    }
    containers.Add(new ContentContainer(content, ContentContainer.ContentLayout.Vertical));
  }

  private static ICodexWidget GetIconWidget(object entity) => (ICodexWidget) new CodexImage(32, 32, Def.GetUISprite(entity));

  private static void GenerateCreatureDescriptionContainers(
    GameObject creature,
    List<ContentContainer> containers)
  {
    if (creature.GetDef<RobotBatteryMonitor.Def>() != null)
    {
      float num = Db.Get().traits.Get(creature.GetComponent<Modifiers>().initialTraits[0]).SelfModifiers.Find((Predicate<AttributeModifier>) (match => match.AttributeId == Db.Get().Amounts.InternalBattery.maxAttribute.Id)).Value;
      containers.Add(new ContentContainer(new List<ICodexWidget>()
      {
        (ICodexWidget) new CodexSpacer(),
        (ICodexWidget) new CodexText((string) CODEX.HEADERS.INTERNALBATTERY, CodexTextStyle.Subtitle),
        (ICodexWidget) new CodexText("    • " + string.Format((string) CODEX.ROBOT_DESCRIPTORS.BATTERY.CAPACITY, (object) num))
      }, ContentContainer.ContentLayout.Vertical));
    }
    if (creature.GetDef<StorageUnloadMonitor.Def>() != null)
      containers.Add(new ContentContainer(new List<ICodexWidget>()
      {
        (ICodexWidget) new CodexSpacer(),
        (ICodexWidget) new CodexText((string) CODEX.HEADERS.INTERNALSTORAGE, CodexTextStyle.Subtitle),
        (ICodexWidget) new CodexText("    • " + string.Format((string) CODEX.ROBOT_DESCRIPTORS.STORAGE.CAPACITY, (object) creature.GetComponents<Storage>()[1].Capacity()))
      }, ContentContainer.ContentLayout.Vertical));
    List<GameObject> prefabsWithTag = Assets.GetPrefabsWithTag((creature.PrefabID().ToString() + "Egg").ToTag());
    if (prefabsWithTag != null && prefabsWithTag.Count > 0)
    {
      containers.Add(new ContentContainer(new List<ICodexWidget>()
      {
        (ICodexWidget) new CodexSpacer(),
        (ICodexWidget) new CodexText((string) CODEX.HEADERS.HATCHESFROMEGG, CodexTextStyle.Subtitle)
      }, ContentContainer.ContentLayout.Vertical));
      foreach (GameObject go in prefabsWithTag)
        containers.Add(new ContentContainer(new List<ICodexWidget>()
        {
          (ICodexWidget) new CodexIndentedLabelWithIcon(go.GetProperName(), CodexTextStyle.Body, Def.GetUISprite((object) go))
        }, ContentContainer.ContentLayout.Horizontal));
    }
    TemperatureVulnerable component = creature.GetComponent<TemperatureVulnerable>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      containers.Add(new ContentContainer(new List<ICodexWidget>()
      {
        (ICodexWidget) new CodexSpacer(),
        (ICodexWidget) new CodexText((string) CODEX.HEADERS.COMFORTRANGE, CodexTextStyle.Subtitle),
        (ICodexWidget) new CodexText("    • " + string.Format((string) CODEX.CREATURE_DESCRIPTORS.TEMPERATURE.COMFORT_RANGE, (object) GameUtil.GetFormattedTemperature(component.internalTemperatureWarning_Low), (object) GameUtil.GetFormattedTemperature(component.internalTemperatureWarning_High))),
        (ICodexWidget) new CodexText("    • " + string.Format((string) CODEX.CREATURE_DESCRIPTORS.TEMPERATURE.NON_LETHAL_RANGE, (object) GameUtil.GetFormattedTemperature(component.internalTemperatureLethal_Low), (object) GameUtil.GetFormattedTemperature(component.internalTemperatureLethal_High)))
      }, ContentContainer.ContentLayout.Vertical));
    List<Tag> tagList1 = new List<Tag>();
    CreatureCalorieMonitor.Def def = creature.GetDef<CreatureCalorieMonitor.Def>();
    if (def != null && def.diet.infos.Length != 0)
    {
      if (tagList1.Count == 0)
        containers.Add(new ContentContainer(new List<ICodexWidget>()
        {
          (ICodexWidget) new CodexSpacer(),
          (ICodexWidget) new CodexText((string) CODEX.HEADERS.DIET, CodexTextStyle.Subtitle)
        }, ContentContainer.ContentLayout.Vertical));
      ContentContainer contentContainer = new ContentContainer();
      contentContainer.contentLayout = ContentContainer.ContentLayout.GridTwoColumn;
      contentContainer.content = new List<ICodexWidget>();
      foreach (Diet.Info info in def.diet.infos)
      {
        if (info.consumedTags.Count != 0)
        {
          foreach (Tag consumedTag in info.consumedTags)
          {
            Element elementByHash = ElementLoader.FindElementByHash(ElementLoader.GetElementID(consumedTag));
            GameObject go = (GameObject) null;
            if (elementByHash.id == SimHashes.Vacuum || elementByHash.id == SimHashes.Void)
            {
              go = Assets.GetPrefab(consumedTag);
              if ((UnityEngine.Object) go == (UnityEngine.Object) null)
                continue;
            }
            if (elementByHash != null && (UnityEngine.Object) go == (UnityEngine.Object) null)
            {
              if (!tagList1.Contains(elementByHash.tag))
              {
                tagList1.Add(elementByHash.tag);
                contentContainer.content.Add((ICodexWidget) new CodexIndentedLabelWithIcon(elementByHash.name, CodexTextStyle.Body, Def.GetUISprite((object) elementByHash.substance)));
              }
            }
            else if ((UnityEngine.Object) go != (UnityEngine.Object) null && !tagList1.Contains(go.PrefabID()))
            {
              tagList1.Add(go.PrefabID());
              contentContainer.content.Add((ICodexWidget) new CodexIndentedLabelWithIcon(go.GetProperName(), CodexTextStyle.Body, Def.GetUISprite((object) go)));
            }
          }
        }
      }
      containers.Add(contentContainer);
    }
    bool flag = false;
    if (def == null || def.diet == null)
      return;
    foreach (Diet.Info info in def.diet.infos)
    {
      if (info.producedElement != (Tag) (string) null)
      {
        flag = true;
        break;
      }
    }
    if (!flag)
      return;
    ContentContainer contentContainer1 = new ContentContainer();
    contentContainer1.contentLayout = ContentContainer.ContentLayout.GridTwoColumn;
    contentContainer1.content = new List<ICodexWidget>();
    ContentContainer contentContainer2 = new ContentContainer(new List<ICodexWidget>()
    {
      (ICodexWidget) new CodexSpacer(),
      (ICodexWidget) new CodexText((string) CODEX.HEADERS.PRODUCES, CodexTextStyle.Subtitle)
    }, ContentContainer.ContentLayout.Vertical);
    containers.Add(contentContainer2);
    List<Tag> tagList2 = new List<Tag>();
    for (int index = 0; index < def.diet.infos.Length; ++index)
    {
      if (def.diet.infos[index].producedElement != Tag.Invalid && !tagList2.Contains(def.diet.infos[index].producedElement))
      {
        tagList2.Add(def.diet.infos[index].producedElement);
        contentContainer1.content.Add((ICodexWidget) new CodexIndentedLabelWithIcon(def.diet.infos[index].producedElement.ProperName(), CodexTextStyle.Body, Def.GetUISprite((object) def.diet.infos[index].producedElement)));
      }
    }
    containers.Add(contentContainer1);
    containers.Add(new ContentContainer(new List<ICodexWidget>()
    {
      (ICodexWidget) new CodexSpacer(),
      (ICodexWidget) new CodexSpacer()
    }, ContentContainer.ContentLayout.Vertical));
  }

  private static void GenerateDiseaseDescriptionContainers(
    Klei.AI.Disease disease,
    List<ContentContainer> containers)
  {
    List<ICodexWidget> content = new List<ICodexWidget>();
    content.Add((ICodexWidget) new CodexSpacer());
    foreach (Descriptor quantitativeDescriptor in disease.GetQuantitativeDescriptors())
      content.Add((ICodexWidget) new CodexText(quantitativeDescriptor.text));
    content.Add((ICodexWidget) new CodexSpacer());
    containers.Add(new ContentContainer(content, ContentContainer.ContentLayout.Vertical));
  }

  private static void GenerateFoodDescriptionContainers(
    EdiblesManager.FoodInfo food,
    List<ContentContainer> containers)
  {
    containers.Add(new ContentContainer(new List<ICodexWidget>()
    {
      (ICodexWidget) new CodexText(food.Description),
      (ICodexWidget) new CodexSpacer(),
      (ICodexWidget) new CodexText(string.Format((string) UI.CODEX.FOOD.QUALITY, (object) GameUtil.GetFormattedFoodQuality(food.Quality))),
      (ICodexWidget) new CodexText(string.Format((string) UI.CODEX.FOOD.CALORIES, (object) GameUtil.GetFormattedCalories(food.CaloriesPerUnit))),
      (ICodexWidget) new CodexSpacer(),
      (ICodexWidget) new CodexText(food.CanRot ? string.Format((string) UI.CODEX.FOOD.SPOILPROPERTIES, (object) GameUtil.GetFormattedTemperature(food.PreserveTemperature), (object) GameUtil.GetFormattedCycles(food.SpoilTime)) : UI.CODEX.FOOD.NON_PERISHABLE.ToString()),
      (ICodexWidget) new CodexSpacer()
    }, ContentContainer.ContentLayout.Vertical));
  }

  private static void GenerateTechDescriptionContainers(
    Tech tech,
    List<ContentContainer> containers)
  {
    containers.Add(new ContentContainer(new List<ICodexWidget>()
    {
      (ICodexWidget) new CodexText((string) Strings.Get("STRINGS.RESEARCH.TECHS." + tech.Id.ToUpper() + ".DESC")),
      (ICodexWidget) new CodexSpacer()
    }, ContentContainer.ContentLayout.Vertical));
  }

  private static void GenerateGenericDescriptionContainers(
    string description,
    List<ContentContainer> containers)
  {
    containers.Add(new ContentContainer(new List<ICodexWidget>()
    {
      (ICodexWidget) new CodexText(description),
      (ICodexWidget) new CodexSpacer()
    }, ContentContainer.ContentLayout.Vertical));
  }

  private static void GenerateBuildingDescriptionContainers(
    BuildingDef def,
    List<ContentContainer> containers)
  {
    List<ICodexWidget> content = new List<ICodexWidget>();
    content.Add((ICodexWidget) new CodexText((string) Strings.Get("STRINGS.BUILDINGS.PREFABS." + def.PrefabID.ToUpper() + ".EFFECT")));
    content.Add((ICodexWidget) new CodexSpacer());
    List<Descriptor> allDescriptors = GameUtil.GetAllDescriptors(def.BuildingComplete);
    List<Descriptor> requirementDescriptors = GameUtil.GetRequirementDescriptors(allDescriptors);
    if (requirementDescriptors.Count > 0)
    {
      content.Add((ICodexWidget) new CodexText((string) CODEX.HEADERS.BUILDINGREQUIREMENTS, CodexTextStyle.Subtitle));
      foreach (Descriptor descriptor in requirementDescriptors)
        content.Add((ICodexWidget) new CodexTextWithTooltip("    " + descriptor.text, descriptor.tooltipText));
      content.Add((ICodexWidget) new CodexSpacer());
    }
    List<Descriptor> effectDescriptors = GameUtil.GetEffectDescriptors(allDescriptors);
    if (effectDescriptors.Count > 0)
    {
      content.Add((ICodexWidget) new CodexText((string) CODEX.HEADERS.BUILDINGEFFECTS, CodexTextStyle.Subtitle));
      foreach (Descriptor descriptor in effectDescriptors)
        content.Add((ICodexWidget) new CodexTextWithTooltip("    " + descriptor.text, descriptor.tooltipText));
      content.Add((ICodexWidget) new CodexSpacer());
    }
    content.Add((ICodexWidget) new CodexText("<i>" + (string) Strings.Get("STRINGS.BUILDINGS.PREFABS." + def.PrefabID.ToUpper() + ".DESC") + "</i>"));
    containers.Add(new ContentContainer(content, ContentContainer.ContentLayout.Vertical));
  }

  private static void GenerateImageContainers(
    Sprite[] sprites,
    List<ContentContainer> containers,
    ContentContainer.ContentLayout layout)
  {
    List<ICodexWidget> content = new List<ICodexWidget>();
    foreach (Sprite sprite in sprites)
    {
      if (!((UnityEngine.Object) sprite == (UnityEngine.Object) null))
      {
        CodexImage codexImage = new CodexImage(128, 128, sprite);
        content.Add((ICodexWidget) codexImage);
      }
    }
    containers.Add(new ContentContainer(content, layout));
  }

  private static void GenerateImageContainers(
    Tuple<Sprite, Color>[] sprites,
    List<ContentContainer> containers,
    ContentContainer.ContentLayout layout)
  {
    List<ICodexWidget> content = new List<ICodexWidget>();
    foreach (Tuple<Sprite, Color> sprite in sprites)
    {
      if (sprite != null)
      {
        CodexImage codexImage = new CodexImage(128, 128, sprite);
        content.Add((ICodexWidget) codexImage);
      }
    }
    containers.Add(new ContentContainer(content, layout));
  }

  private static void GenerateImageContainers(Sprite sprite, List<ContentContainer> containers) => containers.Add(new ContentContainer(new List<ICodexWidget>()
  {
    (ICodexWidget) new CodexImage(128, 128, sprite)
  }, ContentContainer.ContentLayout.Vertical));

  public static void CreateUnlockablesContentContainer(SubEntry subentry) => subentry.lockedContentContainer = new ContentContainer(new List<ICodexWidget>()
  {
    (ICodexWidget) new CodexText((string) CODEX.HEADERS.SECTION_UNLOCKABLES, CodexTextStyle.Subtitle),
    (ICodexWidget) new CodexDividerLine()
  }, ContentContainer.ContentLayout.Vertical)
  {
    showBeforeGeneratedContent = false
  };

  private static void GenerateFabricatorContainers(
    GameObject entity,
    List<ContentContainer> containers)
  {
    ComplexFabricator component = entity.GetComponent<ComplexFabricator>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return;
    containers.Add(new ContentContainer(new List<ICodexWidget>()
    {
      (ICodexWidget) new CodexSpacer(),
      (ICodexWidget) new CodexText((string) Strings.Get("STRINGS.CODEX.HEADERS.FABRICATIONS"), CodexTextStyle.Subtitle),
      (ICodexWidget) new CodexDividerLine()
    }, ContentContainer.ContentLayout.Vertical));
    List<ICodexWidget> content = new List<ICodexWidget>();
    foreach (ComplexRecipe recipe in component.GetRecipes())
      content.Add((ICodexWidget) new CodexRecipePanel(recipe));
    containers.Add(new ContentContainer(content, ContentContainer.ContentLayout.Vertical));
  }

  private static void GenerateReceptacleContainers(
    GameObject entity,
    List<ContentContainer> containers)
  {
    SingleEntityReceptacle plot = entity.GetComponent<SingleEntityReceptacle>();
    if ((UnityEngine.Object) plot == (UnityEngine.Object) null)
      return;
    containers.Add(new ContentContainer(new List<ICodexWidget>()
    {
      (ICodexWidget) new CodexText((string) Strings.Get("STRINGS.CODEX.HEADERS.RECEPTACLE"), CodexTextStyle.Subtitle),
      (ICodexWidget) new CodexDividerLine()
    }, ContentContainer.ContentLayout.Vertical));
    foreach (Tag depositObjectTag in plot.possibleDepositObjectTags)
    {
      List<GameObject> prefabsWithTag = Assets.GetPrefabsWithTag(depositObjectTag);
      if ((UnityEngine.Object) plot.rotatable == (UnityEngine.Object) null)
        prefabsWithTag.RemoveAll((Predicate<GameObject>) (go =>
        {
          IReceptacleDirection component = go.GetComponent<IReceptacleDirection>();
          return component != null && component.Direction != plot.Direction;
        }));
      foreach (GameObject go in prefabsWithTag)
        containers.Add(new ContentContainer(new List<ICodexWidget>()
        {
          (ICodexWidget) new CodexImage(64, 64, Def.GetUISprite((object) go).first),
          (ICodexWidget) new CodexText(go.GetProperName())
        }, ContentContainer.ContentLayout.Horizontal));
    }
  }
}
