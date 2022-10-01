class searchOptions {
    /**支持的逻辑条件 */
    get supportExpressions() {
        return supportExpressions;
    }
    /**表示哪些将被限制在rule里面的条件 */
    get limitOnRule() {
        return ['U', 'T', 'TS', 'A', 'K', 'P', 'PU', 'O', 'L', 'C', 'S', 'IB', 'IS', 'F'];
    }
    /**标识只支持精确匹配的一些字段 */
    get accurateOnly() {
        return ['L', 'C', 'IB', 'IS'];
    }
    /**所有支持的检索类型 */
    get supportSearchTypes() {
        return [
            { key: 'U', value: '任意字段', filed: '' },
            { key: 'T', value: '标题', filed: '' },
            { key: 'TS', value: '丛书名', filed: '' },
            { key: 'A', value: '作者', filed: '' },
            { key: 'K', value: '主题词', filed: '' },
            { key: 'PU', value: '出版社', filed: '' },
            { key: 'L', value: '中图分类号', filed: '' },
            { key: 'C', value: '学科分类号', filed: '' },
            { key: 'S', value: '摘要', filed: '' },
            { key: 'IB', value: 'ISBN', filed: '' },
            { key: 'O', value: '机构', filed: '' },
            { key: 'IS', value: 'ISSN', filed: '' },
            { key: 'F', value: '基金资助', filed: '' },
            { key: 'IN', value: 'ISBN/ISSN', filed: '' },
            { key: 'BN', value: '索书号', filed: '' },
            { key: 'M', value: '纸本', filed: 'medium' },
            { key: 'MO', value: '限定范围', filed: 'medium_owner' },
            { key: 'Y', value: '日期分布', filed: 'date' },
            { key: 'TY', value: '文献类型', filed: 'type' },
            { key: 'LA', value: '语言', filed: 'language' },
            { key: 'SD', value: '教育学科', filed: '' },
            { key: 'DC', value: '期刊收录', filed: '' },
            { key: 'CD', value: '学位级别', filed: '' },
            { key: 'DT', value: '标准类型', filed: '' },
            { key: 'FL', value: '首字母', filed: '' },
            { key: 'CN', value: '国内统一刊号', filed: '' },
            { key: 'P', value: '出版物名称', filed: '' },
            { key: 'CLC0', value: '中图分类号', filed: '' },
            { key: 'CLC1', value: '中图一级分类', filed: 'subject_clc_g1' },
            { key: 'CLC2', value: '中图二级分类', filed: 'subject_clc_g2' },
            { key: 'ESC0', value: '学科分类号', filed: '' },
            { key: 'ESC1', value: '学科一级分类', filed: 'subject_esc_g1' },
            { key: 'ESC2', value: '学科二级分类', filed: 'subject_esc_g2' },
            { key: 'SI', value: '标准号', filed: '' },
            { key: 'DL', value: '馆藏地', filed: '' },
            { key: 'CB', value: '导师', filed: '' },
            { key: 'DI', value: 'DOI', filed: '' },
            { key: 'OC', value: '机构', filed: '' },
            { key: 'AC', value: '作者', filed: '' },
            { key: 'KC', value: '主题词', filed: '' },
        ];
    }
    /**定义一些自动匹配的类型,即输入的正则表达式满足要求将会直接被返回 */
    get autoMapRegexInfo() {
        return [
            {
                regex: /^(10\.\d{4,5}\/[\S]+[^;,.\s])$/,
                searchType: 'DI',
                title: 'doi'
            },
            {
                regex: new RegExp("^((GB)|(DB)|(GJB))(/T)?[0-9]{4}[--][0-9]{4}$"),
                searchType: 'SI',
                title: '标准号'
            }, {
                regex: /^ZL[0-9]{8}\.[0-9X]$/,
                searchType: 'IS',
                title: '专利号'
            }
        ];
    }
    /**表示主检索的聚类字段 */
    get aggsField() {
        return "10#type;10#language;100#date;10#subject;10#subject_esc_g1;10#subject_esc_g2;10#subject_clc_g1;10#subject_clc_g2;10#medium;10#creator_institution_cluster;10#creator_cluster;10#description_core";
    }
    /**支持的排序 */
    get supportSorts() {
        return [
            { key: '', value: '相关度' },
            { key: 'date:desc', value: '时效性倒序' },
            { key: 'date:asc', value: '时效性正序' },
        ];
    }
    /**对应文献类型 */
    get articleTypes() {
        return [
            { key: 1, value: '图书' },
            { key: 2, value: '期刊' },
            { key: 3, value: '期刊文献' },
            { key: 4, value: '学位论文' },
            { key: 5, value: '标准' },
            { key: 6, value: '会议' },
            { key: 7, value: '专利' },
            { key: 8, value: '法律法规' },
            { key: 9, value: '成果' },
            { key: 10, value: '多媒体' },
            { key: 11, value: '报纸' },
            { key: 14, value: '资讯' },
        ];
    }
    /**语言及其代码 */
    get languagePackages() {
        return [
            { key: 'aa', value: '阿法尔语' },
            { key: 'ab', value: '阿布哈兹语' },
            { key: 'ae', value: '阿维斯陀语' },
            { key: 'af', value: '阿非利堪斯语' },
            { key: 'ak', value: '阿坎语' },
            { key: 'am', value: '阿姆哈拉语' },
            { key: 'an', value: '阿拉贡语' },
            { key: 'ar', value: '阿拉伯语' },
            { key: 'as', value: '阿萨姆语' },
            { key: 'av', value: '阿瓦尔语' },
            { key: 'ay', value: '艾马拉语' },
            { key: 'az', value: '阿塞拜疆语' },
            { key: 'ba', value: '巴什基尔语' },
            { key: 'be', value: '白俄罗斯语' },
            { key: 'bg', value: '保加利亚语' },
            { key: 'bh', value: '比哈尔语' },
            { key: 'bi', value: '比斯拉玛语' },
            { key: 'bm', value: '班巴拉语' },
            { key: 'bn', value: '孟加拉语' },
            { key: 'bo', value: '藏语' },
            { key: 'br', value: '布列塔尼语' },
            { key: 'bs', value: '波斯尼亚语' },
            { key: 'ca', value: '加泰隆语' },
            { key: 'ce', value: '车臣语' },
            { key: 'ch', value: '查莫罗语' },
            { key: 'co', value: '科西嘉语' },
            { key: 'cr', value: '克里语' },
            { key: 'cs', value: '捷克语' },
            { key: 'cu', value: '教会斯拉夫语' },
            { key: 'cv', value: '楚瓦什语' },
            { key: 'cy', value: '威尔士语' },
            { key: 'da', value: '丹麦语' },
            { key: 'de', value: '德语' },
            { key: 'dv', value: '迪维希语' },
            { key: 'dz', value: '不丹语' },
            { key: 'ee', value: '埃维语' },
            { key: 'el', value: '现代希腊语' },
            { key: 'en', value: '英语' },
            { key: 'eo', value: '世界语' },
            { key: 'es', value: '西班牙语' },
            { key: 'et', value: '爱沙尼亚语' },
            { key: 'eu', value: '巴斯克语' },
            { key: 'fa', value: '波斯语' },
            { key: 'ff', value: '富拉语' },
            { key: 'fi', value: '芬兰语' },
            { key: 'fj', value: '斐济语' },
            { key: 'fo', value: '法罗斯语' },
            { key: 'fr', value: '法语' },
            { key: 'fy', value: '弗里西亚语' },
            { key: 'ga', value: '爱尔兰语' },
            { key: 'gd', value: '苏格兰盖尔语' },
            { key: 'gl', value: '加利西亚语' },
            { key: 'gn', value: '瓜拉尼语' },
            { key: 'gu', value: '古吉拉特语' },
            { key: 'gv', value: '马恩岛语' },
            { key: 'ha', value: '豪萨语' },
            { key: 'he', value: '希伯来语' },
            { key: 'hi', value: '印地语' },
            { key: 'ho', value: '希里莫图语' },
            { key: 'hr', value: '克罗地亚语' },
            { key: 'ht', value: '海地克里奥尔语' },
            { key: 'hu', value: '匈牙利语' },
            { key: 'hy', value: '亚美尼亚语' },
            { key: 'hz', value: '赫雷罗语' },
            { key: 'ia', value: '国际语A' },
            { key: 'id', value: '印尼语' },
            { key: 'ie', value: '国际语E' },
            { key: 'ig', value: '伊博语' },
            { key: 'ii', value: '四川彝语' },
            { key: 'ik', value: '依努庇克语' },
            { key: 'io', value: '伊多语' },
            { key: 'is', value: '冰岛语' },
            { key: 'it', value: '意大利语' },
            { key: 'iu', value: '伊努伊特语' },
            { key: 'ja', value: '日语' },
            { key: 'jv', value: '爪哇语' },
            { key: 'ka', value: '格鲁吉亚语' },
            { key: 'kg', value: '刚果语' },
            { key: 'ki', value: '基库尤语' },
            { key: 'kj', value: '宽亚玛语' },
            { key: 'kk', value: '哈萨克语' },
            { key: 'kl', value: '格陵兰语' },
            { key: 'km', value: '高棉语' },
            { key: 'kn', value: '坎纳达语' },
            { key: 'ko', value: '朝鲜语' },
            { key: 'kr', value: '卡努里语' },
            { key: 'ks', value: '克什米尔语' },
            { key: 'ku', value: '库尔德语' },
            { key: 'kv', value: '科米语' },
            { key: 'kw', value: '康沃尔语' },
            { key: 'ky', value: '吉尔吉斯语' },
            { key: 'la', value: '拉丁语' },
            { key: 'lb', value: '卢森堡语' },
            { key: 'lg', value: '干达语' },
            { key: 'li', value: '林堡语' },
            { key: 'ln', value: '林加拉语' },
            { key: 'lo', value: '老挝语' },
            { key: 'lt', value: '立陶宛语' },
            { key: 'lu', value: '卢巴-加丹加语' },
            { key: 'lv', value: '拉脱维亚语' },
            { key: 'md', value: '摩尔多瓦语' },
            { key: 'mg', value: '马达加斯加语' },
            { key: 'mh', value: '马绍尔语' },
            { key: 'mi', value: '毛利语' },
            { key: 'mk', value: '马其顿语' },
            { key: 'ml', value: '马拉亚拉姆语' },
            { key: 'mn', value: '蒙古语' },
            { key: 'mo', value: '摩尔达维亚语' },
            { key: 'mr', value: '马拉提语' },
            { key: 'ms', value: '马来语' },
            { key: 'mt', value: '马耳他语' },
            { key: 'my', value: '缅甸语' },
            { key: 'na', value: '瑙鲁语' },
            { key: 'nb', value: '挪威布克莫尔语' },
            { key: 'nd', value: '北恩德贝勒语' },
            { key: 'ne', value: '尼泊尔语' },
            { key: 'ng', value: '恩敦加语' },
            { key: 'nl', value: '荷兰语' },
            { key: 'nn', value: '新挪威语' },
            { key: 'no', value: '挪威语' },
            { key: 'nr', value: '南恩德贝勒语' },
            { key: 'nv', value: '纳瓦霍语' },
            { key: 'ny', value: '尼扬贾语' },
            { key: 'oc', value: '奥克西唐语' },
            { key: 'oj', value: '奥吉布瓦语' },
            { key: 'om', value: '阿芳·奥洛莫语' },
            { key: 'or', value: '奥利亚语' },
            { key: 'os', value: '奥塞梯语' },
            { key: 'pa', value: '旁遮普语' },
            { key: 'pi', value: '巴利语' },
            { key: 'pl', value: '波兰语' },
            { key: 'ps', value: '普什图语' },
            { key: 'pt', value: '葡萄牙语' },
            { key: 'qu', value: '凯楚亚语' },
            { key: 'rm', value: '利托-罗曼语' },
            { key: 'rn', value: '基隆迪语' },
            { key: 'ro', value: '罗马尼亚语' },
            { key: 'ru', value: '俄语' },
            { key: 'rw', value: '基尼阿万达语' },
            { key: 'sa', value: '梵语' },
            { key: 'sc', value: '撒丁语' },
            { key: 'sd', value: '信德语' },
            { key: 'se', value: '北萨米语' },
            { key: 'sg', value: '桑戈语' },
            { key: 'sh', value: '塞尔维亚-克罗地亚语' },
            { key: 'si', value: '僧加罗语' },
            { key: 'sk', value: '斯洛伐克语' },
            { key: 'sl', value: '斯洛文尼亚语' },
            { key: 'sm', value: '萨摩亚语' },
            { key: 'sn', value: '绍纳语' },
            { key: 'so', value: '索马里语' },
            { key: 'sq', value: '阿尔巴尼亚语' },
            { key: 'sr', value: '塞尔维亚语' },
            { key: 'ss', value: '塞斯瓦替语' },
            { key: 'st', value: '塞索托语' },
            { key: 'su', value: '巽他语' },
            { key: 'sv', value: '瑞典语' },
            { key: 'sw', value: '斯瓦希里语' },
            { key: 'ta', value: '泰米尔语' },
            { key: 'te', value: '泰卢固语' },
            { key: 'tg', value: '塔吉克语' },
            { key: 'th', value: '泰语' },
            { key: 'ti', value: '提格里尼亚语' },
            { key: 'tk', value: '土库曼语' },
            { key: 'tl', value: '他加禄语' },
            { key: 'tn', value: '塞茨瓦纳语' },
            { key: 'to', value: '汤加语' },
            { key: 'tr', value: '土耳其语' },
            { key: 'ts', value: '宗加语' },
            { key: 'tt', value: '塔塔尔语' },
            { key: 'tw', value: '特威语' },
            { key: 'ty', value: '塔希提语' },
            { key: 'ug', value: '维吾尔语' },
            { key: 'uk', value: '乌克兰语' },
            { key: 'ur', value: '乌尔都语' },
            { key: 'uz', value: '乌兹别克语' },
            { key: 've', value: '文达语' },
            { key: 'vi', value: '越南语' },
            { key: 'vo', value: '沃拉普克语' },
            { key: 'wa', value: '沃伦语' },
            { key: 'wo', value: '沃洛夫语' },
            { key: 'xh', value: '科萨语' },
            { key: 'yi', value: '依地语' },
            { key: 'yo', value: '约鲁巴语' },
            { key: 'za', value: '壮语' },
            { key: 'zh', value: '中文' },
            { key: 'zu', value: '祖鲁语' }
        ];
    }
}
/**用来处理检索表达式相关的类
 * 注意，查看ts文件可确定具体传入的参数类型
 * 标记为private的方法请勿在外部调用
 * 检索、高级检索和表达式检索应该用3个不同的实例来处理
 */
export class searchExpressionCore {
    constructor() {
        this._addSimpleSearchConditions = [];
        this._inRulesSearchType = searchOption.limitOnRule;
        this._fuzzyCoditionRegex = /([A-Z0-9]{1,3})=([\s\S]+)/;
        this._accurateCoditionRegex = /([A-Z0-9]{1,3})=\[([\s\S]+)\]/;
        this._prefixCoditionRegex = /([A-Z0-9]{1,3})=\[([\s\S]+)\*\]/;
        this._userInputCondition = null;
        /**保留，添加筛选条件 */
        this._userInputFilterRuleCondition = null;
    }
    /**生成不重复的参数名 */
    ensureRandomKeyMaper(contidion, index, symbol) {
        let time = new Date().getTime();
        if (symbol == null)
            symbol = 'f';
        return { key: `${contidion.searchType}_${time}_${index}_${symbol}`, value: contidion };
    }
    /**生成rule表达式 */
    buildRuleStringImpl(rules) {
        if (rules.length == 0)
            return { ruleBody: '', ruleParameters: [] };
        let groupResult = groupBy(rules, x => x.searchType);
        let finalResult = groupResult.map(x => {
            let temp = x.values.map((y, i) => this.ensureRandomKeyMaper(y, i));
            let result = temp.map(y => {
                if (y.value.matchType == searchMatchType.Prefix)
                    return `${y.value.searchType}={@${y.key}}`;
                if (y.value.matchType == searchMatchType.Accurate)
                    return `${y.value.searchType}=[@${y.key}]`;
                return `${y.value.searchType}=@${y.key}`; //默认是模糊匹配
            }).join('[+]');
            if (x.values.length > 1)
                result = `(${result})`;
            return { key: result, value: temp };
        });
        return {
            ruleBody: finalResult.map(x => x.key).join('[*]'),
            ruleParameters: flatten(finalResult.map(x => x.value.map(y => {
                var _a, _b;
                return { value: (_b = (_a = y.value.value) === null || _a === void 0 ? void 0 : _a.toString()) !== null && _b !== void 0 ? _b : '', key: `@${y.key}` };
            })))
        };
    }
    replaceAll(input, match, replacement) {
        return input.split(match).join(replacement);
    }
    /**将后台的检索表达式转换成可供阅读的项 */
    resolveExpressionForDisplayStirngImpl(expression) {
        let result = expression;
        if (IsNullOrWhiteSpace(expression))
            return result;
        for (let index = 0; index < supportExpressions.length; index++) {
            let element = supportExpressions[index];
            result = this.replaceAll(result, element.value, element.displayTitle);
        }
        let arr = searchOption.supportSearchTypes.sort((a, b) => b.key.length - a.key.length);
        for (let index = 0; index < arr.length; index++) {
            let element1 = arr[index];
            result = this.replaceAll(result, element1.key + '=', element1.value + '='); //添加一个=以最大可能保证处于关键词部分
        }
        return result;
    }
    /**将用户通过条件添加进入的部分组装成结果 */
    buildSearchConditionApiRulesImpl() {
        if (this._addSimpleSearchConditions.length == 0)
            return { rule: { ruleBody: '', ruleParameters: [] }, filterRule: { ruleBody: '', ruleParameters: [] } };
        let rules = this._addSimpleSearchConditions.filter(x => this._inRulesSearchType.findIndex(y => y == x.searchType) != -1);
        let filterRules = this._addSimpleSearchConditions.filter(x => this._inRulesSearchType.findIndex(y => y == x.searchType) == -1);
        return { rule: this.buildRuleStringImpl(rules), filterRule: this.buildRuleStringImpl(filterRules) };
    }
    clearBrackets(value) {
        let result = value;
        while (result.startsWith("(") && result.endsWith(")")) {
            result = result.substring(1, result.length - 1);
        }
        result = result
            .replace(/\(([A-Z0-9]{1,3}=)/, "$1")
            .replace(/(=[\s\S]+?)\)/, "$1");
        return result;
    }
    /**将高级表达式转换成后台接口是哟的表达式字符串
      * U=cad[-]A=ng[*](TY=4)[*](DC=SSCI)[*](LA=zh[+]LA=en)[+]Y=2009-2022
     */
    mapHighLevelExpression(displayExpression) {
        if (IsNullOrWhiteSpace(displayExpression))
            return displayExpression;
        for (let index = 0; index < supportExpressions.length; index++) {
            const element = supportExpressions[index];
            displayExpression = displayExpression.replace(element.key, element.value);
        }
        return displayExpression;
    }
    /**将当前的用户录入的filterrule转换成表达式 */
    resolveParameterizedRule2SearchConditions(condition) {
        if (condition == null)
            return [];
        return condition.ruleBody.split('[+]')
            .map(x => {
            let temp = x.split('=');
            let para = condition.ruleParameters.find(y => temp[1].indexOf(y.key) != -1);
            if (para == null)
                para = { key: '', value: temp[1] };
            return {
                searchType: temp[0],
                matchType: searchMatchType.Accurate,
                value: para.value
            };
        });
    }
    /**将后台的检索表达式转换成可供阅读的项 */
    resolveExpressionForDisplay(expression) {
        if (typeof expression == "string") {
            return this.resolveExpressionForDisplayStirngImpl(expression);
        }
        let concatExpressions = expression.ruleBody;
        if (IsNullOrWhiteSpace(concatExpressions) || !IsNotEmptyArray(expression.ruleParameters))
            return this.resolveExpressionForDisplayStirngImpl(concatExpressions);
        let result = this.resolveExpressionForDisplayStirngImpl(concatExpressions);
        for (let index = 0; index < expression.ruleParameters.length; index++) {
            const element = expression.ruleParameters[index];
            let key = element.key;
            if (!key.startsWith('@'))
                key = "@" + key;
            result = result
                .replace(`{${key}}`, element.value)
                .replace(`[${key}]`, element.value)
                .replace(key, element.value);
        }
        return result;
    }
    findMinIndexOf(input, position) {
        let temp = supportExpressions.map(x => {
            return { key: input.indexOf(x.key, position), value: x };
        })
            .filter(x => x.key != -1)
            .sort((a, b) => a.key - b.key);
        if (temp.length == 0)
            return { key: -1, value: null };
        return temp[0];
    }
    comsumeEachPartUserConditionString(partInput, consumedIndex, conditionTempArray) {
        return partInput
            .replace(this._fuzzyCoditionRegex, (match, p1, p2) => {
            let condition = {
                matchType: searchMatchType.Fuzzy,
                searchType: p1,
                value: p2
            };
            let rightBrackets = '';
            while (condition.value.endsWith(')')) {
                rightBrackets += ')';
                condition.value = condition.value.substring(0, condition.value.length - 1);
            }
            let par = this.ensureRandomKeyMaper(condition, consumedIndex, 'u');
            conditionTempArray.push(par);
            return `${p1}=@${par.key}${rightBrackets}`;
        });
    }
    /**确定用户输入的表达式是否成功 */
    verifyUserCondition(input) {
        if (IsNullOrWhiteSpace(input))
            return true;
        let needVerifyArray = [this.clearBrackets(input)];
        for (let index = 0; index < supportExpressions.length; index++) {
            let element = supportExpressions[index];
            needVerifyArray = flatten(needVerifyArray.map(x => x.split(element.key).map(y => this.clearBrackets(y))));
        }
        for (let index = 0; index < needVerifyArray.length; index++) {
            let element = needVerifyArray[index];
            if (this._fuzzyCoditionRegex.test(element)
                || this._accurateCoditionRegex.test(element)
                || this._prefixCoditionRegex.test(element)) {
                if (this._inRulesSearchType.findIndex(x => x == element.substring(0, element.indexOf('='))) == -1)
                    return false;
            }
            else
                return false;
        }
        return true;
    }
    /**添加用户手动录入的条件 */
    addUserConditionString(input) {
        if (IsNullOrWhiteSpace(input)) {
            this._userInputCondition = null;
            return;
        }
        if (!this.verifyUserCondition(input))
            throw new Error('在调用该方法以前，请先通过verifyUserCondition确定用户录入正确');
        let conditionTempArray = [];
        let body = '';
        let consumedIndex = 0;
        let remainString = input;
        let subIndex = this.findMinIndexOf(input, consumedIndex);
        if (subIndex.key == -1) {
            body += this.comsumeEachPartUserConditionString(remainString, consumedIndex, conditionTempArray);
        }
        while (subIndex.key != -1) {
            let tempExpression = remainString.substring(0, subIndex.key);
            //用户输入都是模糊匹配
            body += this.comsumeEachPartUserConditionString(tempExpression, consumedIndex, conditionTempArray);
            body += subIndex.value.value;
            consumedIndex += tempExpression.length;
            consumedIndex += subIndex.value.key.length;
            remainString = input.substring(consumedIndex);
            subIndex = this.findMinIndexOf(remainString, 0);
            if (subIndex.key == -1 && !IsNullOrWhiteSpace(remainString)) {
                tempExpression = remainString;
                body += this.comsumeEachPartUserConditionString(tempExpression, consumedIndex, conditionTempArray);
            }
        }
        this._userInputCondition = {
            ruleBody: body, ruleParameters: conditionTempArray.map(x => {
                return { key: x.key, value: x.value.value };
            })
        };
    }
    /**依次添加检索条件，并用特定的连接符连接 */
    appendRuleCondtion(conditions) {
        if (conditions == null || conditions.length == 0)
            return;
        let ruleBody = '';
        let ruleParameters = [];
        for (let index = 0; index < conditions.length; index++) {
            const element = conditions[index];
            let temp = this.buildRuleStringImpl([element.condition]);
            ruleBody += element.concatExpression;
            ruleBody += temp.ruleBody;
            ruleParameters = ruleParameters.concat(temp.ruleParameters);
        }
        this._userInputCondition = { ruleBody: ruleBody, ruleParameters: ruleParameters };
    }
    /**添加参数化的检索条件 */
    appendParameterizedRuleContidion(condition) {
        this._userInputCondition = condition;
        return this;
    }
    /**添加参数化的筛选条件 */
    appendParameterizedFilterRuleContidion(condition) {
        //this._userInputFilterRuleCondition = condition;
        this.resolveParameterizedRule2SearchConditions(condition)
            .forEach(x => this.addSimpleSearchCondition(x));
        return this;
    }
    /**清空检索条件 */
    clearConditions() {
        this._addSimpleSearchConditions.length = 0;
        this._userInputCondition = null;
        this._userInputFilterRuleCondition = null;
    }
    /**显示检索条件 */
    showRuleExpression() {
        let temp = this.buildApiRules();
        return this.resolveExpressionForDisplay(temp.rule);
    }
    /**返回当前所有的filterrule */
    showFilterRule() {
        let filterRules = this._addSimpleSearchConditions.filter(x => this._inRulesSearchType.findIndex(y => y == x.searchType) == -1);
        if (this._userInputFilterRuleCondition != null) {
            filterRules = filterRules.concat(this.resolveParameterizedRule2SearchConditions(this._userInputFilterRuleCondition));
        }
        return filterRules;
    }
    /**添加一个检索条件 */
    addSimpleSearchCondition(condition) {
        this.removeSearchCondition(condition); //避免重复添加
        this._addSimpleSearchConditions.push(condition);
        return this;
    }
    /**移除一个检索条件 */
    removeSearchCondition(condition) {
        this._addSimpleSearchConditions = this._addSimpleSearchConditions
            .filter(x => !(x.searchType == condition.searchType && x.value == condition.value));
    }
    /**
     * 通过指定的条件生成检索接口用的表达式字符串
     * @returns
     */
    buildApiRules() {
        let result = this.buildSearchConditionApiRulesImpl();
        if (this._userInputCondition != null) {
            if (IsNullOrWhiteSpace(result.rule.ruleBody))
                result.rule.ruleBody = this._userInputCondition.ruleBody;
            else
                result.rule.ruleBody += `[*]${this._userInputCondition.ruleBody}`;
            result.rule.ruleParameters = result.rule.ruleParameters.concat(this._userInputCondition.ruleParameters);
        }
        if (this._userInputFilterRuleCondition != null) {
            if (IsNullOrWhiteSpace(result.filterRule.ruleBody))
                result.filterRule.ruleBody = this._userInputFilterRuleCondition.ruleBody;
            else
                result.filterRule.ruleBody += `[*]${this._userInputFilterRuleCondition.ruleBody}`;
            result.filterRule.ruleParameters = result.filterRule.ruleParameters.concat(this._userInputFilterRuleCondition.ruleParameters);
        }
        return result;
    }
}
/**检索的一些配置项 */
export const searchOption = new searchOptions();
/**检索词匹配模式 */
export var searchMatchType;
(function (searchMatchType) {
    /**精确匹配 */
    searchMatchType[searchMatchType["Accurate"] = 0] = "Accurate";
    /**前向匹配 */
    searchMatchType[searchMatchType["Prefix"] = 1] = "Prefix";
    /**模糊匹配 */
    searchMatchType[searchMatchType["Fuzzy"] = 2] = "Fuzzy";
})(searchMatchType || (searchMatchType = {}));
const supportExpressions = [
    { key: " AND ", value: '[*]', displayTitle: ' 并且 ' },
    { key: ' NOT ', value: '[-]', displayTitle: ' 非 ' },
    { key: ' OR ', value: '[+]', displayTitle: ' 或者 ' }
];
/**判断某字符串是否为空或者全部由空格组成 */
function IsNullOrWhiteSpace(input) {
    if (input == null || input.length == 0)
        return true;
    for (let index = 0; index < input.length; index++) {
        const element = input[index];
        if (element != ' ')
            return false;
    }
    return true;
}
/**表示某个数组不为空 */
function IsNotEmptyArray(arr) {
    return (arr === null || arr === void 0 ? void 0 : arr.length) > 0;
}
/**实现c#中的groupby方法 */
export function groupBy(arr, groupFunc) {
    let result = [];
    for (let index = 0; index < arr.length; index++) {
        const element = arr[index];
        let temp1 = groupFunc(element);
        let temp2 = result.find(x => x.key == temp1);
        if (temp2 == null) {
            temp2 = { key: temp1, values: [] };
            result.push(temp2);
        }
        temp2.values.push(element);
    }
    return result;
}
/**将数组展平 */
export function flatten(arr) {
    return arr.reduce(function (pre, cur) {
        if (!Array.isArray(cur)) {
            return [...pre, cur];
        }
        else {
            return [...pre, ...flatten(cur)];
        }
    }, []);
}
