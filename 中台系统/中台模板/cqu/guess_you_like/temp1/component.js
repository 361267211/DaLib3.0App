function cqu_guess_you_like_temp1() {
  var template_html = `<div class="index-warp-box"><div class="index-like-box">
  <div class="index-like-title">
    <div>
      <span class="index-like-title-red">猜你喜欢</span>
      <span></span>
    </div>
    <span class="index-like-title-more" @click="cqu_change">换一批</span>
  </div>
  <div class="index-like-cont">
    <div class="index-like-list" v-for="(item,index) in cqu_list" :key="index" @click="cqu_linkTo(item.detailLink)">
      <div class="index-like-list-title">
        <span :class="cqu_articleTypes(item.type).bgcolor">{{cqu_articleTypes(item.type).name}}</span>
        <h5>{{item.title}}</h5>
      </div>
      <div class="index-like-list-cont">
        <span v-for="(item1,index1) in typeInfoData[item.type]" :key="index1" v-if="item[item1.value]&&item[item1.value]!=''">{{ item1.name }}:{{ item[item1.value] }}</span>
      </div>
    </div>

    <div class="temp-loading" v-if="request_of"></div><!--加载中-->
    <div class="web-empty-data"  v-if="!request_of&&cqu_list.length==0" :style="{background: 'url('+fileUrl+'/public/image/data-empty.png) no-repeat center'}" >
    </div><!--暂无数据-->
  </div>
</div></div>`;

  var list = document.getElementsByClassName('cqu_guess_you_like_temp1');
  for (var i = 0; i < list.length; i++) {
    if (list[i].getAttribute('class').indexOf('jl_vip_zt_vray') < 0) {
      list[i].setAttribute('class', 'cqu_guess_you_like_temp1 jl_vip_zt_vray jl_vip_zt_warp');
      var template_temp_set_list = null;
      if (list[i].dataset && list[i].dataset.set) {
        template_temp_set_list = JSON.parse(list[i].dataset.set.replace(/'/g, '"'));
      }

      new Vue({
        el: '#' + list[i].lastChild.id,
        template: template_html,
        data() {
          return {
            imgPath: window.localStorage.getItem('fileUrl')+'/public/image/',//公共图片路径
            fileUrl: window.localStorage.getItem('fileUrl'),//图片地址前缀
            post_url_vip: window.apiDomainAndPort,
            request_of:true,//请求中
            cqu_list: [],
            cqu_all_list: [],
            cqu_curIndex: 0,
            cqu_count:5,
            typeInfoData: {
              "1": [//图书
                { name: "作者", value: "creator" },
                { name: "丛书名", value: "title_series" },
                { name: "版本号", value: "title_edition" },
                { name: "出版社", value: "publisher" },
                { name: "出版年", value: "date" },
              ],
              "2": [//期刊
                { name: "出版社", value: "publisher" },
                { name: "pISSN", value: "identifier_pissn" },
                { name: "eISSN", value: "identifier_eissn" },
              ],
              "3": [//期刊文献
                { name: "作者", value: "creator" },
                { name: "期刊名", value: "source" },
                { name: "年", value: "date" },
                { name: "卷", value: "volume" },
                { name: "期", value: "issue" },
              ],
              "4": [//学位论文
                { name: "作者", value: "creator" },
                { name: "作者单位", value: "creator_institution" },
                { name: "学位级别", value: "creator_degree" },
                { name: "学位授予年度", value: "date" },
              ],
              "5": [//标准
                { name: "标准编号", value: "identifier_standard" },
                { name: "发布年份", value: "date" },
              ],
              "6": [//会议
                { name: "作者", value: "creator" },
                { name: "会议名称", value: "source" },
                { name: "主办单位", value: "creator_release" },
                { name: "年", value: "date" },
              ],
              "7": [//专利
                { name: "发明人", value: "creator" },
                { name: "申请人", value: "applicant" },
                { name: "专利类型", value: "description_type" },
                { name: "公开年", value: "date" },
              ],
              "8": [//法律法规
                { name: "终审法院", value: "publisher" },
                { name: "效力级别", value: "description_type" },
                { name: "颁布年份", value: "date" },
              ],
              "9": [//成果
                { name: "完成人", value: "creator" },
                { name: "完成单位", value: "creator_institution" },
                { name: "成果类型", value: "description_type" },
                { name: "公布年份", value: "date" },
              ],
              "10": [//多媒体
                { name: "主讲人", value: "creator" },
                { name: "发布日期", value: "date_created" },
              ],
              "11": [//报纸
                { name: "作者", value: "creator" },
                { name: "报纸名称", value: "source" },
                { name: "年", value: "date" },
              ],
              "12": [//科技报告
                { name: "作者", value: "creator" },
                { name: "作者单位", value: "creator_institution" },
                { name: "报告类型", value: "description_type" },
                { name: "年", value: "date" },
              ],
              "13": [//产品样本
                { name: "作者", value: "creator" },
                { name: "生产公司", value: "creator_institution" },
                { name: "样本类型", value: "description_type" },
                { name: "年", value: "date" },
              ],
              "14": [//资讯
                { name: "作者", value: "creator" },
                { name: "原文出处", value: "description_source" },
                { name: "栏目名", value: "title_series" },
                { name: "年", value: "date" },
              ],
      
            }
          }
        },
        mounted() {
          var template_temp_data_list = [];
          if(template_temp_set_list){
            for (var i = 0; i < template_temp_set_list.length; i++) {
              var topCount = template_temp_set_list[i].topCount;
              var columnid = template_temp_set_list[i].id;
              var OrderRule = template_temp_set_list[i].sortType;
              template_temp_data_list.push({ count: topCount, columnId: columnid, sortField: OrderRule });
              this.cqu_count = topCount;
            }
          }
          this.cqu_initData(template_temp_data_list);
        },
        methods: {
          cqu_linkTo(url) {
            window.open(url);
          },
          cqu_initData() {
            var post_url = this.post_url_vip + '/gusseuserlike/api/asset/guess-whats-my-interested?pageIndex=1';
            axios({
              url: post_url,
              method: 'GET',
              headers: {
                'Content-Type': 'text/plain',
                'Authorization': 'Bearer ' + window.localStorage.getItem('token'),
              },
            }).then(res => {
              if (res.data && res.data.statusCode == 200) {
                this.cqu_all_list = res.data.data.items;
                this.cqu_change();
              }
              this.request_of = false;
            }).catch(err => {
              console.log(err);
              this.request_of = false;
            });
          },
          cqu_change() {
            this.cqu_list = this.cqu_all_list.slice(this.cqu_curIndex * this.cqu_count, (this.cqu_curIndex + 1) * this.cqu_count);
            this.cqu_curIndex = (this.cqu_all_list.length / this.cqu_count) - 1 > this.cqu_curIndex ? this.cqu_curIndex + 1 : 0;
          },
          cqu_articleTypes(val) {
            switch (val) {
              case 1: return { name: '图书', bgcolor: 'like-list-tag-red' };
              case 2: return { name: '期刊', bgcolor: 'like-list-tag-bule' };
              case 3: return { name: '期刊文献', bgcolor: 'like-list-tag-org' };
              case 4: return { name: '学位论文', bgcolor: 'like-list-tag-red' };
              case 5: return { name: '标准', bgcolor: 'like-list-tag-bule' };
              case 6: return { name: '会议', bgcolor: 'like-list-tag-org' };
              case 7: return { name: '专利', bgcolor: 'like-list-tag-red' };
              case 8: return { name: '法律法规', bgcolor: 'like-list-tag-bule' };
              case 9: return { name: '成果', bgcolor: 'like-list-tag-org' };
              case 10: return { name: '多媒体', bgcolor: 'like-list-tag-red' };
              case 11: return { name: '报纸', bgcolor: 'like-list-tag-bule' };
              case 12: return { name: '科技报告', bgcolor: 'like-list-tag-org' };
              case 13: return { name: '产品样本', bgcolor: 'like-list-tag-red' };
              case 14: return { name: '资讯', bgcolor: 'like-list-tag-bule' };
            }
          }
        },
      });
    }
  }
}
cqu_guess_you_like_temp1()