<template>
  <section class="definition">
    <p v-bind:key="index" v-for="(item,index) in this.definitionParts">{{item}}</p>
  </section>
</template>

<script>
export default {
  props: {
    definition: String
  },
  computed: {
    definitionParts: function() {
      if (this.definition != null) {
        return this.formatDescription(this.definition).split("\n");
      }
      return [];
    }
  },
  methods: {
    formatDescription(stringDescription) {
      //insert newlines before 1., 2.,... if there is none
      return stringDescription.replace(/[^\n\d](?=\d+\.)/, "$&\n");
    }
  }
};
</script>

<style lang="scss">
.description {
  p {
    line-height: 1.4em;
  }
}
</style>