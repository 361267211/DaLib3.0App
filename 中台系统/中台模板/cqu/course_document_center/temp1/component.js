function cqu_course_document_center_temp1() {
  var template_html = `<div class="index-warp-box"><div class="index-course-box">
  <div class="index-course-title">
    <div>
      <span class="index-course-title-red">课程文献中心</span>
      <span></span>
    </div>
    <span class="index-course-title-more" @click="cqu_linkTo(cqu_list.moreUrl)">更多+</span>
  </div>
  <div class="index-course-cont">
    <div class="index-course-list" v-for="(item,index) in (cqu_list.asmObjectList||[])" :key="index" @click="cqu_linkTo(item.detailUrl)">
      <img :src="cqu_getImg(item.cover)" :onerror="default_img">
      <div>{{item.objectName}}</div>
    </div>
    <div class="temp-loading" v-if="request_of"></div><!--加载中-->
    <div class="web-empty-data"  v-else-if="cqu_list.asmObjectList.length==0" :style="{background: 'url('+fileUrl+'/public/image/data-empty.png) no-repeat center'}" >
    </div><!--暂无数据-->
  </div>
</div></div>`;

  var list = document.getElementsByClassName('cqu_course_document_center_temp1');
  for (var i = 0; i < list.length; i++) {
    if (list[i].getAttribute('class').indexOf('jl_vip_zt_vray') < 0) {
      list[i].setAttribute('class', 'cqu_course_document_center_temp1 jl_vip_zt_vray jl_vip_zt_warp');
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
            default_img: 'this.src="'+window.localStorage.getItem('fileUrl')+'/public/image/img-error-194x135.png"',
            post_url_vip: window.apiDomainAndPort,
            request_of:true,//请求中
            cqu_list: {asmObjectList:[]},
          }
        },
        mounted() {
          var template_temp_data_list = [];
          if (template_temp_set_list) {
            for (var i = 0; i < template_temp_set_list.length; i++) {
              var topCount = template_temp_set_list[i].topCount;
              // var columnid = template_temp_set_list[i].id;
              // var OrderRule = template_temp_set_list[i].sortType;
              template_temp_data_list.push({ count: topCount });
            }
          }
          this.cqu_initData(template_temp_data_list);
        },
        methods: {
          cqu_linkTo(url) {
            window.open(url);
          },
          cqu_getImg(img) {
            return img.indexOf('http') != -1 ? img : this.fileUrl +'/'+ img;
          },
          cqu_initData(list) {
            //   var post_url = 'http://192.168.21.46:7403' + '/api/CourseResourceCenter/GetCourseResource/5';// /{count}
            var post_url = this.post_url_vip + '/courseresourcecenter/api/CourseResourceCenter/GetCourseResource/'+list[0].count;// /{count}
            axios({
              url: post_url,
              method: 'GET',
              headers: {
                'Content-Type': 'text/plain',
                'Authorization': 'Bearer ' + window.localStorage.getItem('token'),
              },
            }).then(res => {
              if (res.data && res.data.statusCode == 200) {
                this.cqu_list = res.data.data || {asmObjectList:[]};
              }
              this.request_of = false;
            }).catch(err => {
              console.log(err);
              this.request_of = false;
            });
          },
        },
      });
    }
  }
}
cqu_course_document_center_temp1()