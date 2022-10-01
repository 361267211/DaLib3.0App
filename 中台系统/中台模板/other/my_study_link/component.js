/*****未使用 */
function other_my_study_link() {
  var other_my_study_link_html = `<div class="temp-home1-warp">
<div class="my-study">
    <div class="main-title-temp"><span>我的书斋</span><span class="e-title">/ College library</span></div>
    <div class="num-count-warp c-l">
        <div class="n-c-box" v-for="(it,i) in other_my_study_link_list1">
            <span class="num">{{it.count}}</span>
            <span>{{it.name}}</span>
        </div>
    </div>
    <div class="main-title-temp"><span>我的成果</span></div>
    <div class="like-warp">
        <div class="like-row" v-for="(it,i) in other_my_study_link_list2" v-if="i<6">
            <div class="like-title"><span class="type bg1">{{articleTypes(it.type)}}</span>{{it.title}}</div>
            <div>{{it.creator}},{{it.creator_institution}} {{it.date}}</div>
        </div>
    </div>
</div>
</div>`;


  var list = document.getElementsByClassName('other_my_study_link');
  for (var i = 0; i < list.length; i++) {
    if (list[i].getAttribute('class').indexOf('jl_vip_zt_vray') < 0) {
      list[i].setAttribute('class', 'other_my_study_link jl_vip_zt_vray jl_vip_zt_warp');
      new Vue({
        el: '#' + list[i].lastChild.id,
        template: other_my_study_link_html,
        data() {
          return {
            other_my_study_link_list1: [],
            other_my_study_link_list2: [],
            fileUrl: window.localStorage.getItem('fileUrl'),//图片地址前缀
            post_url_vip: window.apiDomainAndPort,
          }
        },
        mounted() {
          this.other_my_study_link_initData1();
          this.other_my_study_link_initData2();
        },
        methods: {
          other_my_study_link_initData1() {
            axios({ //我的书斋
              url: 'http://192.168.21.36:9001/api/reader/my-study-info',
              method: 'GET',
              headers: {
                'Content-Type': 'text/plain',
                'Authorization': 'Bearer ' + window.localStorage.getItem('token'),
              },
            }).then(res => {
              console.log(res);
              if (res.data && res.data.statusCode == 200) {
                this.other_my_study_link_list1 = res.data.data.items || [];
              }
            }).catch(err => {
              console.log(err);
            });
          },
          other_my_study_link_initData2() {
            axios({ //猜你喜欢
              url: 'http://192.168.21.29:9916/api/asset/guess-whats-my-interested?pageindex=1',
              method: 'GET',
              headers: {
                'Content-Type': 'text/plain',
                'Authorization': 'Bearer ' + window.localStorage.getItem('token'),
              },
            }).then(res => {
              console.log(res);
              if (res.data && res.data.statusCode == 200) {
                this.other_my_study_link_list2 = res.data.data.items || [];
              }
            }).catch(err => {
              console.log(err);
            });
          },
          articleTypes(val) {
            switch (val) {
              case 1: return '图书';
              case 2: return '期刊';
              case 3: return '期刊文献';
              case 4: return '学位论文';
              case 5: return '标准';
              case 6: return '会议';
              case 7: return '专利';
              case 8: return '法律法规';
              case 9: return '成果';
              case 10: return '多媒体';
              case 11: return '报纸';
              case 12: return '科技报告';
              case 13: return '产品样本';
              case 14: return '资讯';
            }
          }
        },
      });
    }
  }
}
other_my_study_link()