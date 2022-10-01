function other_college_library() {
  var college_library_html = `<div class="temp-college-library-warp">
<div class="main-title-temp"><span>学院图书馆</span><span class="e-title"></span></div>
<div class="college-l-content c-l">
    <div class="c-l-branch" v-for="(item,i) in other_college_library_list1" v-if="i<6">
        <div class="c-l-title">
            <img src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABwAAAAaCAYAAACkVDyJAAAE5ElEQVRIS6WWf4hUVRTHv+c+Z97uFrtm5bbr/qEx4uo4u++HElGgZQhRlpkSURZFZaAZJkZGRYLSH4GaihQYihCFYioYgpZR/VPau+/NbIsRw7oEiiFuDQvbbDPvnjjLzOKuo+7o/eu9++47n3POPb8It7hSqdTdzc3NiwEsADADQArAVCJqEtHM3A9ggIj6jDFf0c3yXNddTkSvA3iIiFRF+AUAeQAXAQxVZPtElKk8H6kXSI7jPKGU2k5E0wEUmflLIjoO4EQQBIUrDXBdV87uBTCFmXcNDAxsmDAwk8nckUgkDhDRIwKK4/h9y7J2B0FQtWSMszzP20ZEbwA4Xy6XX81msyfkwISAvu8/yMzfAkgS0WeXL19e19/fX6x1Hel0Omnb9hEiepSZf2DmFWEYXqqevSHQcZzVlmXtkjsxxjwZhqGAa650Oj3Ftu0zRHQvgK3FYnFjb2/vf1cevi7Q87xVAD4lonKhUGjP5/Ojmo4ndnZ23tnU1HRaYMz8ttb641paXRPouu5KpdR+Zh6SKAyCoLEqwPO8l5h5ehiGH0rkS2q0tLRoAB1xHC+NoujotbxQE9jd3Z2eNGnSb8x8npmPK6WerwCV67qblVIbxcVBELSkUimrubn5HBG1MfMyrfXh66XaVcD29vamtra2swAmx3HcrZQ6CGDm8PDwVNu2JQWWAfijWCwu6O3tveh53ndE9DCA1UEQ7L5RXl8F9DxvNRHtMsa8GYbhDs/zzgDoBHCWiOYzc6C1nieCHcfZalnWOmbeprVeL+6tC5hOp2+3bVuqxCWttZQpeJ73O4BZRCRl6mut9dOy77ruPCL6BUCWiO4LgqB0I9hVeej7/goAB4wx68Mw3NrR0dHY2to6xMyi+R6t9WtVob7v/yvPcRy3RVH0z0RgtYBy4YuKxeJdDQ0NwvkCgChxSGu9/ArY9wAWMvMirfWpicLGACsV4m8APVrr+13X3a6UWsvMP2qtpROMLMdxFlqWJcBvgiBYMpF7q5n4XV1dMxKJRJ9YZYw5aVnWPmbOaa39yp09S0SPEdEzUksLhcLkfD4/XI91Yyx0HMexLCtk5mNE9DgzXyCiOVLOlFJbJKmZ2QD4U+pkEAQSTHWv0bTIZDKZZDKZY+ZhIpIg+YiZlxDRPGa+yMybBgcH996MVTVdWonIQQCWWFJpqtLfNhcKhZ23CqpCRy10HGepZVmHxRoAOWPMwVKptH98ta/bh+N+GAFKc00mk1LOrDiOZ9aTV/UqMAL0PG8LEb1rjFkbhuHOqpBMJtOZSCQk2aWU3SaDUBzHG6IoksFozHIcZ74Em1IqWS6XT2ezWekYEmRjFs2ePbutsbHxHIA+rfUc+SolLplMblNKvSLvzJwnor8APCDvcRy7URRFVUmu635CRGuqw1Tln31aa/k/HhM0vu9/AGBTHMdPRVF0xHGc6Uqpk0Qk455o+U41Bbq6umYlEglJBxmeFmutf/J9fx+AF40xR8vl8lvGmKGGhoYdlQr1eRAE0sRHoeR5nnTx81prRzTxfX8PM680xqyKokiEjXfdSL6O2xbBMjKWq/vVtsXMP5dKped6enr65Bu5rvsyEf2qtc7Jxty5c1uVUtNyuZx08JornU7fY9v2GgDTmPlUGIZSc8fcl+/7CWPMe5XJ7QWt9TER9j+K5jPPR+63NwAAAABJRU5ErkJggg==">
            <span>{{item.categoryName}}</span>
        </div>
        <ul>
            <li v-for="it in (item.sdeptServiceItems||[])"><a href="javascript:;" :title="it.userGroupName" @click="other_college_library_openurl(it.url)">{{it.userGroupName}}</a></li>
        </ul>
    </div>
    <div class="temp-loading" v-if="request_of"></div><!--加载中-->
    <div class="web-empty-data" v-if="!request_of && other_college_library_list1 && other_college_library_list1.length==0" :style="{background: 'url('+fileUrl+'/public/image/data-empty.png) no-repeat center'}" >
    </div><!--暂无数据-->
</div>
</div>`;


  var list = document.getElementsByClassName('other_college_library');
  for (var i = 0; i < list.length; i++) {
    if (list[i].getAttribute('class').indexOf('jl_vip_zt_vray') < 0) {
      list[i].setAttribute('class', 'other_college_library jl_vip_zt_vray jl_vip_zt_warp');
      new Vue({
        el: '#' + list[i].lastChild.id,
        template: college_library_html,
        data() {
          return {
            request_of:true,//请求中
            other_college_library_list1: [],
            fileUrl: window.localStorage.getItem('fileUrl'),//图片地址前缀
            post_url_vip: window.apiDomainAndPort,
          }
        },
        mounted() {
          this.other_college_library_initData();
        },
        methods: {
          other_college_library_openurl(url) {
            window.open(url)
          },
          other_college_library_initData() {
            var post_url = this.post_url_vip + '/departmentlib/api/department-liba/all-department-lib';
            axios({
              url: post_url,
              method: 'GET',
              headers: {
                'Content-Type': 'text/plain',
                'Authorization': 'Bearer ' + window.localStorage.getItem('token'),
              },
            }).then(res => {
              this.request_of = false;
              if (res.data && res.data.statusCode == 200) {
                this.other_college_library_list1 = res.data.data || [];
              }
            }).catch(err => {
              this.request_of = false;
              console.log(err);
            });
          },
        },
      });
    }
  }
}
other_college_library()