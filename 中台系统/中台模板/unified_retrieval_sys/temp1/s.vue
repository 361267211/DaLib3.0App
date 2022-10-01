<template>
  <div id="Search" v-if="searchBoxConfigure">
    <div class="top-title" v-if="searchBoxConfigure.displayTitleEnabled">
      {{ searchBoxConfigure.displayTitle }}
    </div>
    <div class="search">
      <div class="search-nav" v-if="searchBoxConfigure.titleItemEnabled">
        <template v-for="item in searchBoxConfigure.searchBoxTitleItems">
          <span v-if="item.type == 0" :key="item.id" @click="changeSearchType(item)" class="search-nav-list" :class="searchTypeItem==item?'type-checked':''">
            {{ item.title }}
          </span>
          <span :key="item.id" v-if="item.type == 1" class="search-nav-list">
            <a :href="item.value" :target="item.navType == 1 ? '_blank' : ''">
              {{ item.title }}</a>
          </span>
        </template>
      </div>
      <div class="head">
        <div class="search-content">
          <div class="head-odiv">
            <img src="@/assets/web/img/sear.png" style="width: 40px" />
            <input type="text" v-model="basicInputKeyWord" @blur="inputBlur" @focus="emptySearch" ref="mainInput" />
          </div>
          <div class="head-font search-btn cursor" @click="basicSearch">
            检索
          </div>
        </div>
        <div>
          <div class="search-right cursor" @click="block = true">高级检索</div>
          <div class="search-right cursor" @click="block = true" v-show="false">检索历史</div>
        </div>
      </div>
      <div class="hot-word-panel" v-if="searchBoxConfigure&&searchBoxConfigure.hotResourceEnabled&&emptySearchModel&&emptySearchModel.hotKeyword&&emptySearchModel.hotKeyword.length>0">
        热门检索：

        <span class="hot-word" v-for="(hotword, index) in emptySearchModel.hotKeyword" :key="index" @click="searchKeyword(hotword.word)">{{hotword.word}}</span>
      </div>
    </div>

    <template v-if="searchBoxConfigure.smartPanelEnabled">
      <div class="content-box" v-if="emptySearchModel.show && !basicInputKeyWord">
        <div class="content">
          <div class="content-middle" v-if="
              emptySearchModel.hotComponent &&
              emptySearchModel.hotComponent.length > 0
            ">
            <div class="content-middle-left">
              <div class="content-middle-left-top">功能推荐</div>
              <div class="content-middle-left-bottom">
                <a class="content-middle-left-bottom-item" v-for="item in emptySearchModel.hotComponent" :key="item.docId" :href="item.url" target="_blank">
                  <i class="iconfont icon-ego-box"></i>
                  {{ item.title }}
                </a>
              </div>
            </div>
          </div>
          <template v-if="
              emptySearchModel.hotKeyword &&
              emptySearchModel.hotKeyword.length > 0
            ">
            <div class="content-about">检索发现</div>
            <div class="content-bottom">
              <div class="content-bottom-item" v-for="(hotword, index) in emptySearchModel.hotKeyword" :key="index" @click="searchKeyword(hotword.word)">
                <i class="iconfont icon-sousuo"></i>
                <div class="content-bottom-item-text">
                  {{ hotword.word }}
                </div>
              </div>
            </div>
          </template>
        </div>
      </div>
      <div class="content-box" v-if="onKeywordInputSuggestModel.show && basicInputKeyWord">
        <div class="content">
          <template v-if="
              onKeywordInputSuggestModel.matchComponent &&
              onKeywordInputSuggestModel.matchComponent.length > 0
            ">
            <div class="content-top">
              <div class="content-top-list nav" v-for="(
                  item, index
                ) in onKeywordInputSuggestModel.matchComponent" :key="index">
                <div>
                  <img src="@/assets/web/img/inco3.png" width="18px" />
                </div>
                <div class="content-top-list-middle">
                  导航至{{ mapServiceComponentName(item.app_type) }}
                </div>
                <a class="cursor" v-html="hightLightShow(item.title)" target="_blank" :href="item.url"></a>
              </div>
            </div>
          </template>
          <div class="content-middle" v-if="onKeywordInputSuggestModel.regexInfo">
            <div class="content-middle-left" @click="regexMatchSearch(onKeywordInputSuggestModel.regexInfo)">
              <div class="content-middle-left-top">
                查找{{ onKeywordInputSuggestModel.regexInfo.title }}
              </div>
              <div class="content-middle-left-bottom">
                <div class="content-middle-left-bottom-item">
                  <i class="iconfont icon-bazi"></i>
                  {{ basicInputKeyWord }}
                </div>
              </div>
            </div>
          </div>
          <template v-if="
              onKeywordInputSuggestModel.autoComplete &&
              onKeywordInputSuggestModel.autoComplete.length > 0
            ">
            <div class="content-about">
              检索“{{ basicInputKeyWord }}”相关文献
            </div>
            <div class="content-bottom">
              <div class="content-bottom-item" v-for="(word, index) in onKeywordInputSuggestModel.autoComplete" :key="index" @click="searchKeyword(word)">
                <i class="iconfont icon-sousuo"></i>
                <div class="content-bottom-item-text">
                  {{ word }}
                </div>
              </div>
            </div>
          </template>
        </div>
      </div>
    </template>
    <div v-if="block" class="bolck-odiv">
      <div class="body">
        <div id="advancedRetrieval">
          <div class="block-content-top">
            <div class="cursor tips">高级检索</div>
            <div class="cursor" style="padding-bottom: 43px" @click="tabSwitch()">表达式检索 </div>
          </div>
          <div class="block-content-middle">
            <template v-for="(item, index) in dynamicExpressionFormModels">
              <div class="block-content-middle-item" :key="index">
                <select class="block-sel1" style="opacity: 0" v-if="index == 0">
                  <option value="volvo">Volvo</option>
                </select>
                <div class="block-sel3" v-else>
                  <select name="cars" v-model="item.concatExpression">
                    <option v-for="expression in supportExpressions" :key="expression.key" :value="expression.value">
                      {{ expression.displayTitle }}
                    </option>
                  </select>
                </div>
                <div class="block-sel2">
                  <select name="cars" v-model="item.supportFied" @change="fiedChange(item)">
                    <option v-for="field in supportFieds" :key="field.key" :value="field.key">
                      {{ field.key }} = {{ field.value }}
                    </option>
                  </select>
                </div>
                <input type="text" v-model="item.keyword" />
                <div class="block-sel3">
                  <select name="cars" v-model="item.searchMatchType">
                    <option v-for="searchMatchTypeKeyValuePair in mapSearchMatchTypes(item.supportFied)" :key="searchMatchTypeKeyValuePair.key" :value="searchMatchTypeKeyValuePair.key">{{ searchMatchTypeKeyValuePair.value }}</option>
                  </select>
                </div>
                <div class="cursor" v-if=" index == dynamicExpressionFormModels.length - 1 && dynamicExpressionFormModels.length < 5 " @click="addDynamicExpression">
                  <img src="@/assets/web/img/jiajian.png" style="width: 30px" />
                </div>
                <div class="cursor" v-else style="visibility: hidden;">
                  <img src="@/assets/web/img/jiajian.png" style="width: 30px" />
                </div>
                <div class="cursor" v-if="index != 0" @click="removeDynamicExpresion(item)">
                  <img src="@/assets/web/img/jiajian1.png" style="width: 30px" />
                </div>
                <div class="cursor" v-else style="visibility: hidden;">
                  <img src="@/assets/web/img/jiajian1.png" style="width: 30px" />
                </div>
              </div>
            </template>

            <div class="block-content-middle-li cursor block-row-content" style="padding-top: 10px">
              <div class="color-red">文献类型:</div>
              <div class="title-right">
                <template v-for="(articleType, idx) in articleTypes">
                  <label :key="articleType.value" :class="idx == 0 ? 'block-first' : 'block-box-lefts'">
                    <input type="checkbox" v-model="articleType.checked" />
                    <span> {{ articleType.displayTitle }}</span>
                  </label>
                </template>
              </div>
            </div>
            <div class="block-content-middle-li block-content-middle-li-left">
              <div class="color-red">出版时间:</div>
              <div class="block-content-middle-li-xz">
                <select name="cars" class="block-content-middle-li-xz-section" v-model="yearRange[0]">
                  <option v-for="year in yearAsc" :key="year" :value="year"> {{ year }}</option>
                </select>
              </div>
              <div class="block-line"></div>
              <div class="">
                <select name="cars" class="block-content-section" v-model="yearRange[1]">
                  <option v-for="year in yearDesc" :key="year" :value="year">
                    {{ year }}
                  </option>
                </select>
              </div>
            </div>
            <div class="block-content-middle-li block-row-content" style="margin: 14px 0" v-if="supportDescriptionCore && supportDescriptionCore.length > 0">
              <div class="color-red">核心收录:</div>
              <div class="title-right">
                <template v-for="(description,idx) in supportDescriptionCore">
                  <label :key="description.value" :class="idx == 0 ? 'block-first' : 'block-box-lefts'">
                    <input type="checkbox" v-model="description.checked" />
                    <span> {{ description.displayTitle }}</span>
                  </label>
                </template>
              </div>
            </div>
            <div class="block-content-middle-li block-row-content">
              <div class="color-red">文献语言:</div>
              <div class="title-right">
                <template v-for="(langurage, idx) in supportLanguageFilterRule">
                  <label :key="langurage.value" :class="idx == 0 ? 'block-first' : 'block-box-lefts'">
                    <input type="checkbox" v-model="langurage.checked" />
                    <span> {{ langurage.displayTitle }}</span>
                  </label>
                </template>
              </div>
            </div>
            <div class="block-content-middle-li block-row-content">
              <div class="color-red">文献载体:</div>
              <div class="title-right">
                <template v-for="(medium, idx) in supportMediumRange">
                  <label :key="medium.value" :class="idx == 0 ? 'block-first' : 'block-box-lefts'">
                    <input type="checkbox" v-model="medium.checked" />
                    <span> {{ medium.displayTitle }}</span>
                  </label>
                </template>
              </div>
            </div>
          </div>
          <div class="block-content-bottom">
            <button class="block-content-bottom-btn cursor" style="background-color: #c2c2c2" @click="block = false">
              取消
            </button>
            <button class="block-content-bottom-btn cursor" @click="highLevelSearchClick">检索</button>
          </div>
        </div>
      </div>
    </div>

    <div v-if="block_s" class="bolck-box">
      <div class="ex-body">
        <div class="ex-content">
          <div class="ex-content-top">
            <div style="padding-bottom: 43px" class="cursor" @click="tabSwitch_s()">
              高级检索
            </div>
            <div class="cursor hot">表达式检索</div>
          </div>
          <div class="search-content-height">
            <div class="ex-content-middle">
              <textarea rows="5" cols="20" v-model="personalExpressInput">
            </textarea>
              <div class="ex-content-middle-xz cursor">
                <div class="ex-content-middle-li block-row-content">
                  <div class="color-red">文献类型:</div>
                  <div class="title-right">
                    <template v-for="(articleType, idx) in articleTypes_s">
                      <label :key="articleType.value" :class="idx == 0 ? 'block-first' : 'block-box-lefts'">
                        <input type="checkbox" v-model="articleType.checked" />
                        <span> {{ articleType.displayTitle }}</span>
                      </label>
                    </template>
                  </div>
                </div>
                <div class="ex-content-middle-li block-row-content" v-if=" supportDescriptionCore_s && supportDescriptionCore_s.length > 0">
                  <div class="color-red">核心收录:</div>
                  <div class="title-right">
                    <template v-for="(description,idx) in supportDescriptionCore_s">
                      <label :key="description.value" :class="idx == 0 ? 'block-first' : 'block-box-lefts'">
                        <input type="checkbox" v-model="description.checked" />
                        <span> {{ description.displayTitle }}</span>
                      </label>
                    </template>
                  </div>
                </div>
              </div>
              <div class="hint-warp">
                <p>
                <div class="hint-title">文字说明:</div>
                <div class="hint-content">
                  T=题名（书名、题名），A=作者（责任者），K=主题词，P=出版物名称，PU=出版社名称，O=机构（作者单位、学位授予单位、专利申请人），L=中图分类号，C=学科分类号，U=全部字段
                </div>
                </p>
                <p>
                <div class="hint-title">检索规则说明:</div>
                <div class="hint-content">
                  AND代表“并且”；OR代表“或者”；NOT代表“不包含”；(注意必须大写,运算符两边需空一格)
                </div>
                </p>
                <p>
                <div class="hint-title">检索范例:</div>
                <div class="hint-content">
                  范例一：(K=图书馆学 OR K=情报学) AND A=范并思
                  <br />
                  范例二：P=计算机应用与软件 AND (U=C++ OR U=Basic) NOT K=Visual
                </div>
                </p>
              </div>
              <div class="block-content-bottom">
                <button class="block-content-bottom-btn cursor" style="background-color: #c2c2c2" @click="block_s = false">
                  取消
                </button>
                <button class="block-content-bottom-btn cursor" @click="personalExpressSearchClick" :disabled="!personalExpressInput||!validUserInputCondition">检索</button>
              </div>
            </div>
          </div>
          <!-- <div class="ex-content-bottom">
            <div class="ex-content-bottom-word ex-content-bottom-word-sty">
              <div class="ex-content-bottom-word-box">
                <div class="ex-content-bottom-word-box-top ex-text">
                  文字说明:
                </div>
                <div id="ex-content-bottom-word-box-top-des ex-top">
                  T=题名（书名、题名），A=作者（责任者），K=主题词，P=出版物名称，PU=出版社名称，O=机构（作者单位、学位授予单位、专利申请人），L=中图分类号，C=学科分类号，U=全部字段
                </div>
              </div>
              <div class="ex-content-bottom-word-box">
                <div
                  class="ex-content-bottom-word-box-top"
                  style="color: #a41e1d; padding-top: 25px"
                >
                  检索规则说明:
                </div>
                <div id="ex-content-bottom-word-box-top-des ex-top">
                  AND代表“并且”；OR代表“或者”；NOT代表“不包含”；(注意必须大写,运算符两边需空一格)
                </div>
              </div>
              <div class="ex-content-bottom-word-box">
                <div class="ex-content-bottom-word-box-top ex-text">
                  检索范例:
                </div>
                <div id="ex-content-bottom-word-box-top-des ex-top">
                  范例一：(K=图书馆学 OR K=情报学) AND A=范并思
                  <br />
                  范例二：P=计算机应用与软件 AND (U=C++ OR U=Basic) NOT K=Visual
                </div>
              </div>
            </div>
            <div class="ex-content-bottom-btn">
              <div
                class="cursor"
                @click="block_s = false"
                style="background-color: #c2c2c2"
              >
                取消
              </div>
              <button
                class="cursor"
                @click="personalExpressSearchClick"
                :disabled="!personalExpressInput"
              >
                检索
              </button>
            </div>
          </div> -->
        </div>
      </div>
    </div>
  </div>

</template>

<script>
import {
  searchOption,
  searchExpressionCore,
  searchMatchType,
} from "@/assets/web/js/searchExpressionResolver";

const rxjs = window.rxjs;
export default {
  name: "Search",
  data() {
    return {
      /**对应 AND OR这些鬼东西 */
      supportExpressions: searchOption.supportExpressions,
      /**支持的语言过滤条件,直接用简单表达式的形式 */
      supportLanguageFilterRule: [
        {
          searchType: "LA",
          value: "zh",
          matchType: searchMatchType.Accurate,
          checked: false,
          displayTitle: "中文",
        },
        {
          searchType: "LA",
          value: "en",
          matchType: searchMatchType.Accurate,
          checked: false,
          displayTitle: "英文",
        },
      ],
      /**文献载体的区分 */
      supportMediumRange: [
        {
          searchType: "M",
          value: "2",
          matchType: searchMatchType.Accurate,
          checked: false,
          displayTitle: "电子",
        },
        {
          searchType: "M",
          value: "1",
          matchType: searchMatchType.Accurate,
          checked: false,
          displayTitle: "纸书",
        },
      ],
      /**核心收录 */
      supportDescriptionCore: null,
      articleTypes: searchOption.articleTypes
        .filter((x) => x.key != 2)
        .map((x) => {
          return {
            searchType: "TY",
            value: x.key,
            matchType: searchMatchType.Accurate,
            checked: false,
            displayTitle: x.value,
          };
        }), //文章类型
      /**高级检索中动态表单 */
      dynamicExpressionFormModels: [
        {
          concatExpression: "",
          keyword: "",
          searchMatchType: searchMatchType.Fuzzy,
          supportFied: "T",
        },
        {
          concatExpression: "[+]",
          keyword: "",
          searchMatchType: searchMatchType.Accurate,
          supportFied: "A",
        },
      ],
      //年份
      yearRange: [1900, new Date().getFullYear()],
      searchMatchTypes: [
        { key: searchMatchType.Accurate, value: "精确" },
        { key: searchMatchType.Fuzzy, value: "模糊" },
        { key: searchMatchType.Prefix, value: "前向" },
      ],
      searchBoxConfigure: null,
      //主检索处理器
      searchExpressionResolver: new searchExpressionCore(),
      /**检索条件为空的时候出来的提示面板 */
      emptySearchModel: { show: false, hotComponent: null, hotKeyword: null },
      /**当输入关键词的时候提示的项 */
      onKeywordInputSuggestModel: {
        show: false,
        autoComplete: null,
        regexInfo: null,
        matchComponent: null,
      },
      articleTypes_s: searchOption.articleTypes.map((x) => {
        return {
          searchType: "TY",
          value: x.key,
          matchType: searchMatchType.Accurate,
          checked: false,
          displayTitle: x.value,
        };
      }),
      /**表达式检索的checkbox与高级检索的分开 */
      supportDescriptionCore_s: null,
      //用户自定义的检索表达式
      personalExpressInput: "",
      /**选中的检索项 */
      searchTypeItem: null,
      /**绑定到检索关键字 */
      basicInputKeyWord: "",
      block: false,
      block_s: false,
      baseUrl: "",
      webBase: location.origin + "/articlesearch",
    };
  },
  mounted() {
    if (!rxjs)
      throw new Error(
        "rxjs未正确加载，请确保有对应script标签，src地址https://cdn.bootcdn.net/ajax/libs/rxjs/6.0.0/rxjs.umd.js"
      );
    this.initComponentAsync().then(() => {
      this.emptySearch().then(() => (this.emptySearchModel.show = false)); //获取热门检索
      if (this.$refs.mainInput)
        rxjs
          .fromEvent(this.$refs.mainInput, "keyup")
          .pipe(
            rxjs.operators.map(() => this.basicInputKeyWord),
            rxjs.operators.debounceTime(200), //防抖
            rxjs.operators.throttleTime(200), //节流
            rxjs.operators.distinctUntilChanged()
          )
          .subscribe((x) => {
            this.rxAutoComplete(x);
          });
    });
  },
  computed: {
    /**支持的检索字段 */
    supportFieds: function () {
      return searchOption.limitOnRule

        .map((x) => {
          let temp = searchOption.supportSearchTypes.find((y) => y.key == x);

          return temp;
        })
        .filter((x) => x);
    },
    /**支持的筛选年份，正序 */
    yearAsc: function () {
      let yearEnd = new Date().getFullYear();
      let yearStart = 1900;
      return Array.from(
        { length: yearEnd - yearStart + 1 },
        (x, i) => yearStart + i
      );
    },
    yearDesc: function () {
      let yearEnd = new Date().getFullYear();
      let yearStart = 1900;
      return Array.from(
        { length: yearEnd - yearStart + 1 },
        (x, i) => yearEnd - i
      );
    },
    validUserInputCondition: function () {
      return this.searchExpressionResolver.verifyUserCondition(
        this.personalExpressInput
      );
    },
  },
  methods: {
    fiedChange(item) {


      if (item.supportFied == 'A' || searchOption.accurateOnly.find(x => x == item.supportFied) != null) item.searchMatchType = searchMatchType.Accurate;
      else item.searchMatchType = searchMatchType.Fuzzy;
    },
    /**不通的字段匹配不同的范围条件 */
    mapSearchMatchTypes(searchType) {

      if (searchOption.accurateOnly.find(x => x == searchType) != null)
        return this.searchMatchTypes.filter(
          (x) => x.key == searchMatchType.Accurate
        );
      if (searchType == "A")
        return this.searchMatchTypes.filter(
          (x) => x.key != searchMatchType.Fuzzy
        );
      if (searchType == "U") return this.searchMatchTypes.filter(x => x.key != searchMatchType.Prefix);
      return this.searchMatchTypes;
    },
    /**检索框失去焦点 */
    inputBlur() {
      setTimeout(() => {
        this.emptySearchModel.show = false;
        this.onKeywordInputSuggestModel.show = false;
      }, 200);
    },
    /**初始化页面的一些数据 */
    initComponentAsync() {
      return Promise.all([
        this.getJsonAsync("/api/search-box/configure").then((x) => {
          this.searchBoxConfigure = x.data;
          if (
            this.searchBoxConfigure &&
            this.searchBoxConfigure.searchBoxTitleItems
          )
            this.searchTypeItem =
              this.searchBoxConfigure.searchBoxTitleItems.find(
                (y) => y.type == 0
              );
        }),
        this.getJsonAsync(
          "/api/search-const/available-database-collect-kind"
        ).then((x) => {
          if (x.data) {
            this.supportDescriptionCore = x.data.map((y) => {
              return {
                searchType: "DC",
                value: y.englishAbbr.toLowerCase(),
                matchType: searchMatchType.Accurate,
                checked: false,
                displayTitle: y.databaseName,
              };
            });
            this.supportDescriptionCore_s = x.data.map((y) => {
              return {
                searchType: "DC",
                value: y.englishAbbr.toLowerCase(),
                matchType: searchMatchType.Accurate,
                checked: false,
                displayTitle: y.databaseName,
              };
            });
          }
        }),
      ]);
    },
    /**基本检索 */
    basicSearch() {
      if (!this.basicInputKeyWord) return;

      if (this.searchTypeItem && this.searchTypeItem.symbol == "COMPONENT") {
        let href = `${this.webBase}/#/search/webSiteSearch?keyword=${this.basicInputKeyWord}`;
        if (Number(this.searchTypeItem.value) > 0) {
          href += `&type=${this.searchTypeItem.value}`;
        }
        window.location.href = href;

        window.location.reload();
        return;
      }
      this.searchExpressionResolver.clearConditions();
      this.searchExpressionResolver.addSimpleSearchCondition({
        searchType: "U",
        value: this.basicInputKeyWord,
        matchType: searchMatchType.Fuzzy,
      });
      if (this.searchTypeItem && this.searchTypeItem.symbol != "U") {
        this.searchExpressionResolver.addSimpleSearchCondition({
          searchType: this.searchTypeItem.symbol,
          value: this.searchTypeItem.value,
          matchType: searchMatchType.Accurate,
        });
      }
      this.goToSearch();
    },
    /**自定义检索 */
    personalExpressSearchClick() {
      this.searchExpressionResolver.clearConditions();
      if (
        !this.searchExpressionResolver.verifyUserCondition(
          this.personalExpressInput
        )
      ) {
        alert("表达式录入错误，请修改后提交");
        return;
      }
      console.log(this.personalExpressInput);

      this.searchExpressionResolver.addUserConditionString(
        this.personalExpressInput
      );
      this.supportDescriptionCore_s
        .filter((x) => x.checked)
        .concat(this.articleTypes_s.filter((x) => x.checked))
        .forEach((x) =>
          this.searchExpressionResolver.addSimpleSearchCondition(x)
        );

      this.goToSearch();
    },
    /**高级检索 */
    highLevelSearchClick() {
      this.searchExpressionResolver.clearConditions();

      let dynamicExpression = this.dynamicExpressionFormModels.filter(
        (x) => x.keyword
      );
      if (dynamicExpression.length == 0) {
        alert("请至少输入一个检索条件");
        return;
      }
      dynamicExpression[0].concatExpression = ""; //忽略第一项的连接符
      this.searchExpressionResolver.appendRuleCondtion(
        dynamicExpression.map((x) => {
          return {
            concatExpression: x.concatExpression,
            condition: {
              searchType: x.supportFied,
              value: x.keyword,
              matchType: x.searchMatchType,
            },
          };
        })
      );

      let filterRules = []
        .concat(this.articleTypes.filter((x) => x.checked))
        .concat(this.supportDescriptionCore.filter((x) => x.checked))
        .concat(this.supportLanguageFilterRule.filter((x) => x.checked))
        .concat(this.supportMediumRange.filter((x) => x.checked));
      filterRules.push({
        searchType: "Y",
        value: "[" + this.yearRange.join(" TO ") + "]",
        matchType: searchMatchType.Accurate,
        checked: false,
        displayTitle: "年份",
      });

      filterRules.forEach((x) =>
        this.searchExpressionResolver.addSimpleSearchCondition(x)
      );

      this.goToSearch();
    },
    /**添加一个动态条件 */
    addDynamicExpression() {
      this.dynamicExpressionFormModels.push({
        concatExpression: "[+]",
        keyword: "",
        searchMatchType: searchMatchType.Fuzzy,
        supportFied: "U",
      });
    },
    /**移除一个条件 */
    removeDynamicExpresion(item) {
      let index = this.dynamicExpressionFormModels.indexOf(item);
      if (index != -1) this.dynamicExpressionFormModels.splice(index, 1);
    },
    /**进入到检索结果页面 */
    goToSearch() {
      this.postJsonAsync(
        "/api/search-const/encrypt-search-parameter",
        this.searchExpressionResolver.buildApiRules()
      ).then((x) => {
        let keyword = this.basicInputKeyWord || "";
        if (keyword.length >= 100) keyword = keyword.substring(0, 100);
        let href = `${this.webBase}/#/searchResult?key=${x.data
          }&sid=${this.searchBoxConfigure.id}&keyword=${encodeURIComponent(keyword)}`;
        debugger
        window.location.href = href;
        location.reload();
      });
    },
    /**正则匹配上后调用 */
    regexMatchSearch(regexInfo) {
      this.searchExpressionResolver.addSimpleSearchCondition({
        searchType: regexInfo.searchType,
        value: this.basicInputKeyWord,
        matchType: searchMatchType.Fuzzy,
      });
      this.goToSearch();
    },
    /**适用于rxjs防抖节流策略的下拉框 */
    rxAutoComplete(x) {
      let _this = this;
      if (x == null) return;
      Promise.all([
        Promise.resolve(
          searchOption.autoMapRegexInfo.find((y) => y.regex.test(x))
        ),
        this.postJsonAsync("/api/search/match-component", {
          pageIndex: 1,
          pageSize: 5,
          keyword: x,
        }),
        this.getJsonAsync("/api/search/auto-complete", {
          limit: 10,
          keyword: x,
        }),
      ])
        .then((resultArray) => {
          _this.onKeywordInputSuggestModel.regexInfo = resultArray[0];
          _this.onKeywordInputSuggestModel.matchComponent =
            resultArray[1].data &&
              resultArray[1].data.hits &&
              resultArray[1].data.hits.source &&
              resultArray[1].data.hits.source.length > 0
              ? resultArray[1].data.hits.source
              : null;
          _this.onKeywordInputSuggestModel.autoComplete =
            resultArray[2].data && resultArray[2].data.length > 0
              ? resultArray[2].data
              : null;
          if (
            !_this.onKeywordInputSuggestModel.regexInfo &&
            !_this.onKeywordInputSuggestModel.matchComponent &&
            !_this.onKeywordInputSuggestModel.autoComplete
          )
            _this.onKeywordInputSuggestModel.show = false;
          else _this.onKeywordInputSuggestModel.show = true;
        })
        .catch(() => {
          _this.onKeywordInputSuggestModel.show = false;
        });
    },

    /**自动补全或者热门关键词的检索 */
    searchKeyword(keyword) {
      if (!keyword) return;
      this.searchExpressionResolver.clearConditions();
      this.searchExpressionResolver.addSimpleSearchCondition({
        searchType: "U",
        value: keyword,
        matchType: searchMatchType.Fuzzy,
      });
      this.basicInputKeyWord = keyword;
      this.goToSearch();
    },
    mapServiceComponentName(type) {
      switch (parseInt(type)) {
        case 1 << 1:
          return "服务";
        case 1 << 2:
          return "功能";
        case 1 << 3:
          return "数据库";
        case 1 << 4:
          return "问题";
        case 1 << 5:
          return "新闻";
        case 1 << 6:
          return "回答";
        case 1 << 7:
          return "宣传图片";

        default:
          break;
      }
    },
    //当输入框获取焦点的时候
    emptySearch() {
      if (this.emptySearchModel.hotComponent) {
        this.emptySearchModel.show = true;
        return Promise.resolve(null); //将热门组件和检索词缓存起来
      }
      return Promise.all([
        this.getJsonAsync("/api/search/hot-component", { limit: 4, sid: this.searchBoxConfigure.id }),
        this.getJsonAsync("/api/search/hot-words", { limit: 8 }),
      ]).then((resultArray) => {
        this.emptySearchModel.hotComponent =
          resultArray[0].data && resultArray[0].data.length > 0
            ? resultArray[0].data
            : null;
        if (
          resultArray[1].data &&
          resultArray[1].data.hits &&
          resultArray[1].data.hits.source
        )
          this.emptySearchModel.hotKeyword = resultArray[1].data.hits.source;
        else this.emptySearchModel.hotKeyword = null;
        this.emptySearchModel.show =
          this.emptySearchModel.hotKeyword ||
          this.emptySearchModel.hotComponent;
      });
    },
    /**检索框的检索切换 */
    changeSearchType(item) {
      this.searchTypeItem = item;
    },
    /**显示关键字高亮 */
    hightLightShow(message) {
      if (this.basicInputKeyWord == null) return "";
      if (!message) return message;
      let highlightToken = this.basicInputKeyWord;
      return message
        .toLowerCase()
        .split(highlightToken)
        .join(`<i class='reds'>${highlightToken}</i>`);
    },

    //向指定的连接发起get请求
    async getJsonAsync(url, querys) {
      if (!url.startsWith("/")) url = "/" + url;
      let requestUrl = this.baseUrl + url;
      if (querys != null) {
        let queryString = Object.keys(querys)
          .map((x) => `${x}=${querys[x]}`)
          .join("&");
        if (queryString) requestUrl += `?${queryString}`;
      }

      return axios({
        url: requestUrl,
        method: "GET",
      }).then((response) => response.data);
    },
    /**向指定的连接发起Post请求 */
    async postJsonAsync(url, data) {
      if (!url.startsWith("/")) url = "/" + url;
      let requestUrl = this.baseUrl + url;
      if (data == null) data = {};

      return axios({
        url: requestUrl,
        method: "POST",
        data: data,
      }).then((response) => response.data);
    },

    tabSwitch() {
      this.block = false;
      this.block_s = true;
    },
    tabSwitch_s() {
      this.block = true;
      this.block_s = false;
    },
  },
};
</script>
 